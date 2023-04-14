import type { MilochauCoreOptions } from "@amilochau/core-vue3"
import { getConfig } from "../utils/config"
import routes from "./routes"
import navigation from "./navigation"

export enum Environment {
  Default = 'default',
  Local = 'local',
  Development = 'dev',
  Production = 'prd'
}

export type EnvConfigValues = {
  [key in Environment]: Record<string, string>
}

export const defaultEnv: Environment = Environment.Default

export const envConfig: EnvConfigValues = {
  default: {
  },
  local: {
    VITE_API_URL: "http://localhost:4000",
    VITE_COGNITO_USERPOOL_ID: "eu-west-3_91PfBkcmP",
    VITE_COGNITO_CLIENT_ID: '@todo',
  },
  dev: {
    VITE_API_URL: "https://@todo.cloudfront.net/api", // @todo to adapt when domain is supported
    VITE_COGNITO_USERPOOL_ID: "eu-west-3_91PfBkcmP",
    VITE_COGNITO_CLIENT_ID: '@todo',
  },
  prd: {
    VITE_API_URL: "https://@todo.cloudfront.net/api", // @todo to adapt when domain is supported
    VITE_COGNITO_USERPOOL_ID: "eu-west-3_yAqixEcS4",
    VITE_COGNITO_CLIENT_ID: '@todo',
  }
}

export const getCurrentEnv = (host: string, subdomain: string): Environment => {
  if (host.includes('localhost')) {
    return Environment.Local
  } else if (subdomain.includes('dev') || subdomain.includes('@todo')) { // @todo to remove when domain is supported
    return Environment.Development
  } else {
    return Environment.Production
  }
}

export const coreOptions: MilochauCoreOptions = {
  application: {
    name: 'Contacts',
    contact: 'Antoine Milochau',
    navigation,
    onAppBarTitleClick: router => router.push({ name: 'Home' })
  },
  api: {
    gatewayUri: getConfig('VITE_API_URL')
  },
  i18n: {
    messages: {
      en: {
        appTitle: 'Contacts'
      },
      fr: {
        appTitle: 'Contacts'
      }
    }
  },
  identity: {
    cognito: {
      userPoolId: getConfig('VITE_COGNITO_USERPOOL_ID'),
      clientId: getConfig('VITE_COGNITO_CLIENT_ID'),
    },
  },
  routes: routes,
  clean: () => {
    return () => {
    }
  }
}

export const authorizedDomains = [
  'localhost',
  'milochau.com'
]
