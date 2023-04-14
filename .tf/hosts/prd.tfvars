conventions = {
  application_name = "contacts"
  host_name        = "dev"
}

cognito_clients_settings = {
  "client" = {
    purpose = "Web UI"
  }
}

lambda_settings = {
  base_directory = "../src/contacts-api/functions"
  functions = {
    "http/messages/post" = {
      memory_size_mb = 256
      http_triggers = [{
        method = "POST"
        route  = "/api/messages"
      }]
    }
  }
}

dynamodb_tables_settings = {
  "messages" = {
    partition_key = "id"
    ttl = {
      enabled = true
    }
  }
}

client_settings = {
  package_source_file = "../src/contacts-client/dist"
}
