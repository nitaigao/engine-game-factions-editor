require 'find'
require 'zip/zipfilesystem'
require '../etc/scripts/version'

class MSBuild
  def initialize(solutionFile, clrVersion = 'v3.5')
    @solutionFile = solutionFile
    frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', clrVersion)
    @msbuildExecutable = File.join(frameworkDir, 'msbuild.exe')
  end
  
  def clean(compileMode = 'Debug')
      sh "#{@msbuildExecutable} #{@solutionFile} /property:Configuration=#{compileMode} /t:Clean"
  end
  
	def compile(compileMode = 'Debug')
		sh "#{@msbuildExecutable} #{@solutionFile} /m /property:BuildInParallel=false /property:Configuration=#{compileMode} /t:Rebuild"
	end
end

def delete_empty_directories(root_directory)
  iterations = deleted_total = 0
  loop do # a directory containing an directory will become empty when the subdir is removed
    iterations += 1
    deleted = 0
    Dir[File.join(root_directory,'**','*')].each do |e|
      next unless File.directory?(e)
      next unless (Dir.entries(e)-['.','..']).empty?
      Dir.delete(e) # will fail if e is not, in fact, an empty dir
      deleted += 1
    end
    break if deleted.zero?
    deleted_total += deleted
  end
end

namespace :build do
	desc "Cleans the solution"
	task :clean do
		begin
			MSBuild.new($solution_file).clean( $build_configuration )
		rescue Exception => e
			raise "\n\nFailed: There was an error when trying to clean the solution\n#{e}"
		end
	end
	
	desc "Builds the solution"
	task :compile do
		begin
			MSBuild.new($solution_file).compile( $build_configuration )
		rescue Exception => e
			raise "\n\nFailed: There was an error when compiling the solution\n#{e}"
		end
	end
	
	desc "Test the build output"
	task :test do
		begin
		
			#tests_path = "cd ../build/" + $build_configuration.to_s + "/bin & " + $application_name + ".Tests.exe"
			#system( tests_path )
			
			test_files_list = FileList.new( File.join( $outputdir, '**/*Tests.dll' ) )
			
			test_files_list.each { | testfile | sh "nunit-console #{testfile}" }
		    
		rescue Exception => e
			raise "\n\nFailed: There was an error while running tests\n#{e}"
		end
	end
end

namespace :deploy do
  task :clean do
  
	build_file_list = FileList.new( File.join( $outputdir,'**/*.*' ) ).exclude( File.join( $outputdir,'**/*.exe' ), File.join( $outputdir,'**/*.dll' ), File.join( $outputdir,'**/*.cfg' ), File.join( $outputdir,'**/*.bad' ) )
	FileUtils.rm build_file_list, :force => true
	
	test_files_list = FileList.new( File.join( $outputdir, '**/*test*' ), File.join( $outputdir, '**/*cpp*' ), File.join( $outputdir, '**/*mockpp*' ) )
	FileUtils.rm test_files_list, :force => true
	
	delete_empty_directories( $outputdir )
	
  end
  task :store do
	begin	
		versionInfo = VersionInfo.new
		artifacts_path = $packagesdir
		output_path = File.join(artifacts_path, $zipName + '.zip')
	  
	    FileUtils.rm_rf(artifacts_path)
	    Dir.mkdir(artifacts_path)
		
		build_output_dir = $outputdir
	      
	    Zip::ZipFile.open(output_path, Zip::ZipFile::CREATE) do |zipfile|
	      Find.find(build_output_dir) do |path|     			
			if File.directory?(path) == false then       
				dest = path.slice(build_output_dir.length + 1, path.length - build_output_dir.length - 1)
				puts "Adding #{dest} to #{output_path}"
				zipfile.add(dest,path) if dest
			end
	      end
	    end
		
	rescue Exception => e
		raise "\n\nFailed: There was an error while creating the build output\n#{e}"
	end
  end
end


$build_configuration = 'Release'
$application_name = 'DataCompiler'
$solution_file = $application_name + '.sln'
$builddir = '../build'
$packagesdir = File.join( '../', 'packages' )
$outputdir = File.join( $builddir, $build_configuration, 'bin' )
$zipName = 'LevelEditor'

task :build => ["build:clean", "build:compile"]
task :test => ["build:test"]
task :deploy => [ "deploy:clean", "deploy:store" ]
task :continuous_integration => [ :build, :test, :deploy ]
task :default => :build