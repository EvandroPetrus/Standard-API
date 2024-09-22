pipeline {
    agent any

    environment {
        DOTNET_VERSION = '8.0'  // Set your .NET version
    }

    stages {
        stage('Checkout') {
            steps {
                // Checkout the code from the repository
                checkout scm
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Restore NuGet packages
                script {
                    bat "dotnet restore"  // Use 'sh' if running on Linux/Mac
                }
            }
        }

        stage('Build') {
            steps {
                // Build the project
                script {
                    bat "dotnet build --configuration Release"  // Use 'sh' for Linux/Mac
                }
            }
        }

        stage('Test') {
            steps {
                // Run tests
                script {
                    bat "dotnet test"  // Use 'sh' for Linux/Mac
                }
            }
        }

        stage('Publish') {
            steps {
                // Publish the project
                script {
                    bat "dotnet publish --configuration Release --output ./publish"  // Use 'sh' for Linux/Mac
                }
            }
        }

        stage('Deploy') {
            steps {
                // Deploy the app (adjust this step to match your deployment method)
                script {
                    echo 'Deploy step goes here'
                    // For example, deploy using SCP or any other mechanism
                    // bat "scp -r ./publish/* user@remote-server:/path/to/deploy"  // Use your deployment command here
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed. Check the logs.'
        }
    }
}
