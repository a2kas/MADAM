pipeline {
    agent {
        label 'slave1 || slave2'
    }
    environment {
        IMAGE_NAME = "tamro-madam_multibranch"
        DOCKER_REP = "dockerrepoapi.tamro.lt"
        DEPLOYMENT_YAML_PATH = "/deployment/MultiBranch"
        DEPLOY_TO_SERVER_TEST = "kube-test-m.tamro.lt"
        DEPLOY_TO_SERVER_TEST2 = "kube-test2-m.tamro.lt"
        DEPLOY_TO_SERVER_PREPROD = "kube-tam-pp-m.tamro.lt"
        DEPLOY_TO_SERVER_PROD = "kube-tam-m.tamro.lt"
        DEPLOYMENT = "tamro-madam-deployment"
        CONTAINER_NAME_TEST = "tamro-madam-test"
        CONTAINER_NAME_PREPROD = "tamro-madam-preprod"
        CONTAINER_NAME_PROD = "tamro-madam-prod"
        SOURCES = "https://bitbucket.tamro.lt/scm/dat/datamonitor.git"
        LOGINS = credentials('Centos7_logins') // later credentials variable must be used in single quote because of variable interpolations
        TOKEN = credentials('bitbucketBearer')
        NUGET_PASS = credentials('Nuget_pass')
        CONFIGNAME = "tamro-madam-config" //this is used to get appsettings from bitbucket
        SECRETNAME = "tamro-madam-secret" // this is secret in k8s.
        SOURCEJSON = "Tamro.Madam.Ui/appsettings.json"
        SECRETREPO = "TAMRO"
        SLNNAME = "Tamro.Madam.sln"
        MIGRATIONSPROJECTFOLDER = "Tamro.Madam.Repository"
        MIGRATIONSSTARTUPFOLDER = "Tamro.Madam.Ui"
		TESTS_FOLDER = "test/Tamro.Madam.Application.Tests"
		DOTNET_VERSION = "8" // this is needed if project is .net8 because old agents does not support .net and will fail.
		DBCONTEXTTOIGNORE = "Tamro.Madam.Repository.Context.Wholesale.WhRawEeDatabaseContext,Tamro.Madam.Repository.Context.Wholesale.WhRawLtDatabaseContext,Tamro.Madam.Repository.Context.Wholesale.WhRawLvDatabaseContext,Tamro.Madam.Repository.Context.E1Gateway.E1DbContext,Tamro.Madam.Repository.Context.Sks.SksDbContext,Tamro.Madam.Repository.Context.Jpg.JpgDbContext"
    }
    stages {
        //----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------PR-* BRANCHES
        //----------------------------------------------------------------------------------------------------------------------------------------

        //-----------------PR to TEST
        stage("PR- Check appsettings test") { // only for PR to test
            when {
                allOf {
                    branch "PR-*"
                    equals(actual: env.CHANGE_TARGET, expected: 'test')
                }
            }
            steps {
                JenkinsfilePRpart(PrToBranch: "Test")
            }
        }
        //-----------------PR to Preprod
        stage("PR- Check appsettings preprod") { // only for PR to preprod
            when {
                allOf {
                    branch "PR-*"
                    equals(actual: env.CHANGE_TARGET, expected: 'preprod')
                }
            }
            steps {
                JenkinsfilePRpart(PrToBranch: "Preprod")
            }
        }
        //-----------------PR to Prod
        stage("PR- Check appsettings prod") { // only for PR to prod
            when {
                allOf {
                    branch "PR-*"
                    equals(actual: env.CHANGE_TARGET, expected: 'main')
                }
            }
            steps {
                JenkinsfilePRpart(PrToBranch: "Prod,checkIfMainHasHotfix")
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------TEST BRANCH
        //----------------------------------------------------------------------------------------------------------------------------------------
        stage("TEST") { // only for PR to prod
			when {
                branch "test"
            }
			steps{
                JenkinsfileTESTpart(Actions: "Cobertura,BuildContainer,AppsettingsFileActions,ApplyMigrations,DeployContainer",
					DBcontextsToIgnore: "${DBCONTEXTTOIGNORE}")
            }
        }

        stage("TEST-2") { 
			when {
                branch "test2"
            }
			steps{
                JenkinsfileTESTpart(Actions: "BuildContainer,AppsettingsFileActionsTest2,ApplyMigrations,DeployContainerTest2",
					DBcontextsToIgnore: "${DBCONTEXTTOIGNORE}")
            }
        }

				/* disabled since it is not needed
        stage('Build Nuget') {
            when {
                allOf{
                    changeset "Tamro.Madam.Contracts/**"
                    branch "test"
                }
								allOf {
                    anyOf {
                        changeset "Tamro.Madam.Contracts/**"
                        changeset "Tamro.Madam/Controllers/**"
                    }
                    branch "test"
                }
            }
            steps {
                sh label: '', script: 'dotnet build Tamro.Madam.Contracts/Tamro.Madam.Contracts.csproj -c Release'
                sh label: '', script: "dotnet pack Tamro.Madam.Contracts/Tamro.Madam.Contracts.csproj /p:Version=1.0.${env.BUILD_NUMBER} -c Release -o ${env.WORKSPACE}"
                sh label: '', script: "dotnet nuget push Tamro.Madam.Contracts.1.0.${env.BUILD_NUMBER}.nupkg -k ${NUGET_PASS} -s https://nuget.tamro.lt"
            }
        }
				*/

        //----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------PREPROD BRANCH
        //----------------------------------------------------------------------------------------------------------------------------------------
        stage("PREPROD- wait for test branch build") {
            when {
                branch "preprod"
            }
            steps {
                script {
                    BranchName = getBranchFrom()
                    BranchName = BranchName.replace("/", "-")
                    println("Merged from branch: ${BranchName}")

                    if (BranchName.toLowerCase().equals("test")) {
                        Job_Name = env.JOB_NAME.substring(0, env.JOB_NAME.indexOf('/'))
                        waitJobToComplete(
                            JOB_NAME: "${Job_Name}",
                            JOB_BRANCH: "test",
                            MULTIBRANCH: "true",
                            WAIT_SECONDS: "15",)
                    }
                }
            }
        }
        //----------------------------------------------------
        //-----------------PREPROD BRANCH FROM TEST
        //----------------------------------------------------
        stage("PREPROD- from test branch") { // only for PR to prod
			when {
                allOf {
                    branch "preprod"
                    expression {
                        (BranchName.toLowerCase().trim().equals("test"))
                    }
                    expression {
                        !currentBuild.buildCauses.toString().contains('UserIdCause')
                    }
                }
            }
			steps{
                JenkinsfilePREPRODpart(Actions: "AppsettingsFileActions,ApplyMigrations,DeployContainerFromTest",
					DBcontextsToIgnore: "${DBCONTEXTTOIGNORE}")
            }
        }

        //----------------------------------------------------
        //-----------------PREPROD BRANCH FROM ANY OTHER BRANCH
        //----------------------------------------------------
        stage("PREPROD- from other branch") { // only for PR to prod
			when {
                anyOf {
                    allOf {
                        branch "preprod"
                        expression {
                            (!BranchName.toLowerCase().trim().equals("preprod") && !BranchName.toLowerCase().trim().equals("test")) //merge from main to preprod should build image thats why main is not checked here
                        }
                    }
                    allOf {
                        branch "preprod"
                        expression {
                            currentBuild.buildCauses.toString().contains('UserIdCause')
                        }
                    }
                }
            }
			steps{
                JenkinsfilePREPRODpart(Actions: "BuildContainer,AppsettingsFileActions,ApplyMigrations,DeployContainer",
					DBcontextsToIgnore: "${DBCONTEXTTOIGNORE}")
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------main BRANCH
        //----------------------------------------------------------------------------------------------------------------------------------------
        stage("main- wait for preprod branch build") {
            when {
                branch "main"
            }
            steps {
                script {
                    BranchName = getBranchFrom()
                    BranchName = BranchName.replace("/", "-")
                    println("Merged from branch: ${BranchName}")

                    if (BranchName.toLowerCase().equals("preprod")) {
                        Job_Name = env.JOB_NAME.substring(0, env.JOB_NAME.indexOf('/'))
                        waitJobToComplete(
                            JOB_NAME: "${Job_Name}",
                            JOB_BRANCH: "preprod",
                            MULTIBRANCH: "true",
                            WAIT_SECONDS: "15",)
                    }
                }
            }
        }
        //----------------------------------------------------
        //-----------------main BRANCH FROM PREPROD
        //----------------------------------------------------
        stage("main- from preprod branch") { // only for PR to prod
			when {
                allOf {
                    branch "main"
                    expression {
                        (BranchName.toLowerCase().trim().equals("preprod"))
                    }
                    expression {
                        !currentBuild.buildCauses.toString().contains('UserIdCause')
                    }
                }
            }
			steps{
                JenkinsfilePRODpart(Actions: "AppsettingsFileActions,ApplyMigrations,DeployContainerFromPreprod",
					DBcontextsToIgnore: "${DBCONTEXTTOIGNORE}")
            }
        }

        //----------------------------------------------------
        //-----------------main BRANCH FROM ANY OTHER BRANCH
        //----------------------------------------------------
        stage("main- from other branch") { // only for PR to prod
			when {
                anyOf {
                    allOf {
                        branch "main"
                        expression {
                            (!BranchName.toLowerCase().trim().equals("main") && !BranchName.toLowerCase().trim().equals("preprod") && !BranchName.toLowerCase().trim().equals("test"))
                        }
                    }
                    allOf {
                        branch "main"
                        expression {
                            currentBuild.buildCauses.toString().contains('UserIdCause')
                        }
                    }
                }
            }
			steps{
                JenkinsfilePRODpart(Actions: "RunTests,BuildContainer,AppsettingsFileActions,ApplyMigrations,DeployContainer",
					DBcontextsToIgnore: "${DBCONTEXTTOIGNORE}")
            }
        }
    }

    post {
        failure {
            sendBuildStatusEmail()
        }
    }
}
