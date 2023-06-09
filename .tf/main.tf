terraform {
  backend "s3" {
    bucket = "terraform-shd-bucket"
    region = "eu-west-3"
    key    = "terraform.tfstate"

    workspace_key_prefix = "contacts" # To adapt for new projects
    dynamodb_table       = "terraform-shd-table-locks"
  }

  required_providers {
    aws = {
      source = "hashicorp/aws"
    }
  }

  required_version = ">= 1.3.0"
}

provider "aws" {
  region = var.aws_provider_settings.region

  default_tags {
    tags = {
      application = var.conventions.application_name
      host        = var.conventions.host_name
    }
  }
}

provider "aws" {
  alias  = "no-tags"
  region = var.aws_provider_settings.region
}

provider "aws" {
  alias  = "us-east-1"
  region = "us-east-1"

  default_tags {
    tags = {
      application = var.conventions.application_name
      host        = var.conventions.host_name
    }
  }
}

module "checks" {
  source      = "git::https://github.com/amilochau/tf-modules.git//shared/checks?ref=v1"
  conventions = var.conventions
}

module "functions_app" {
  source      = "git::https://github.com/amilochau/tf-modules.git//aws/functions-app?ref=v1"
  conventions = var.conventions

  cognito_clients_settings = var.cognito_clients_settings

  lambda_settings = {
    architecture = "x86_64"
    runtime      = "provided.al2"
    functions = {
      for k, v in var.lambda_settings.functions : "${replace(k, "/", "-")}" => {
        memory_size_mb        = v.memory_size_mb
        timeout_s             = v.timeout_s
        deployment_file_path  = "${var.lambda_settings.base_directory}/${k}/${v.package_file}"
        handler               = v.handler
        environment_variables = v.environment_variables
        http_triggers         = v.http_triggers
        sns_triggers          = v.sns_triggers
        scheduler_triggers    = v.scheduler_triggers
        ses_accesses          = v.ses_accesses
        lambda_accesses       = v.lambda_accesses
      }
    }
  }

  dynamodb_tables_settings = var.dynamodb_tables_settings
}

module "client_app" {
  source      = "git::https://github.com/amilochau/tf-modules.git//aws/static-web-app?ref=v1"
  conventions = var.conventions

  api_settings = {
    domain_name = module.functions_app.apigateway_invoke_domain
    origin_path = module.functions_app.apigateway_invoke_origin_path
  }
  client_settings = {
    package_source_file   = var.client_settings.package_source_file
    s3_bucket_name_suffix = var.client_settings.s3_bucket_name_suffix
    domains               = var.client_settings.domains
  }

  providers = {
    aws.no-tags   = aws.no-tags
    aws.us-east-1 = aws.us-east-1
  }
}
