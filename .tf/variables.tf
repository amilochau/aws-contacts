variable "conventions" {
  description = "Conventions to use"
  type = object({
    application_name = string
    host_name        = string
  })
}

variable "aws_provider_settings" {
  description = "Settings to configure the AWS provider"
  type = object({
    region = optional(string, "eu-west-3")
  })
  default = {}
}

variable "cognito_clients_settings" {
  description = "Settings to configure identity clients for the API"
  type = map(object({
    purpose = string
  }))
  default = {}
}

variable "lambda_settings" {
  description = "Lambda settings"
  type = object({
    base_directory = string
    functions = map(object({
      memory_size_mb        = optional(number, 128)
      timeout_s             = optional(number, 10)
      package_file          = optional(string, "bin/Release/net7.0/linux-x64/publish/bootstrap.zip")
      handler               = optional(string, "bootstrap")
      environment_variables = optional(map(string), {})
      http_triggers = optional(list(object({
        method      = string
        route       = string
        anonymous   = optional(bool, false)
        enable_cors = optional(bool, false)
      })), [])
      sns_triggers = optional(list(object({
        topic_name = string
      })), [])
    }))
  })
}

variable "dynamodb_tables_settings" {
  description = "Settings to configure DynamoDB tables for the API"
  type = map(object({
    partition_key = string
    sort_key      = optional(string, null)
    attributes = optional(map(object({
      type = string
    })), {})
    ttl = optional(object({
      enabled        = bool
      attribute_name = optional(string, "ttl")
      }), {
      enabled = false
    })
    global_secondary_indexes = optional(map(object({
      partition_key      = string
      sort_key           = string
      non_key_attributes = list(string)
    })), {})
  }))
  default = {}
}

variable "client_settings" {
  description = "Client application settings"
  type = object({
    package_source_file = string
    s3_bucket_name_suffix = string
    domains = optional(object({
      zone_name                 = string
      domain_name               = string
      subject_alternative_names = optional(list(string), [])
    }), null)
  })
}
