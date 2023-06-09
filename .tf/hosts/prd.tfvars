conventions = {
  application_name = "contacts"
  host_name        = "prd"
}

cognito_clients_settings = {
  "client" = {
    purpose = "Web UI"
  }
}

lambda_settings = {
  base_directory = "../src/contacts-api/functions"
  functions = {
    "async/unsubscribe-emails" = {}
    "http/messages/post" = {
      memory_size_mb = 256
      http_triggers = [{
        method    = "POST"
        route     = "/api/messages"
        anonymous = false
        }, {
        method    = "POST"
        route     = "/api/a/messages"
        anonymous = true
      }]
    }
    "http/messages/get" = {
      memory_size_mb = 256
      http_triggers = [{
        method    = "GET"
        route     = "/api/messages/{messageId}"
        anonymous = false
        }, {
        method    = "GET"
        route     = "/api/a/messages/{messageId}"
        anonymous = true
      }]
    }
    "scheduler/summary" = {
      scheduler_triggers = [{
        description         = "Send a summary of pending contacts every day"
        schedule_expression = "rate(1 days)"
      }]
      lambda_accesses = [{
        arn = "arn:aws:lambda:eu-west-3:266302224431:function:emails-prd-fn-async-send-emails"
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
    attributes = {
      "st" = {
        type = "N"
      }
      "cd" = {
        type = "N"
      }
    }
    global_secondary_indexes = {
      "by_st_thenby_cd" = {
        partition_key = "st"
        sort_key      = "cd"
        non_key_attributes = [
          "id",
          "user_id",
          "co"
        ]
      }
    }
  }
  "contactusers" = {
    partition_key = "type"
    sort_key      = "id"
  }
}

client_settings = {
  package_source_file = "../src/contacts-client/dist"
  domains = {
    zone_name   = "milochau.com"
    domain_name = "contact.milochau.com"
  }
}
