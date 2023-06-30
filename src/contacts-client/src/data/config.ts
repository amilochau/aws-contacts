import type { MilochauCoreOptions } from "@amilochau/core-vue3"
import { getConfig, getCurrentEnvironment } from "../utils/config"
import routes from "./routes"
import navigationItems from "./navigation"
import logoUrl from "@/assets/logo.png"

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
    VITE_COGNITO_CLIENT_ID: '63891k3n9159vur4sfsn2ntnk0',
  },
  dev: {
    VITE_API_URL: "https://dev.contact.milochau.com/api",
    VITE_COGNITO_USERPOOL_ID: "eu-west-3_91PfBkcmP",
    VITE_COGNITO_CLIENT_ID: '63891k3n9159vur4sfsn2ntnk0',
  },
  prd: {
    VITE_API_URL: "https://contact.milochau.com/api",
    VITE_COGNITO_USERPOOL_ID: "eu-west-3_yAqixEcS4",
    VITE_COGNITO_CLIENT_ID: '61tdr81eljvd2k8ornt4rnuep7',
  }
}

export const getCurrentEnv = (host: string, subdomain: string): Environment => {
  if (host.includes('localhost')) {
    return Environment.Local
  } else if (subdomain.includes('dev')) {
    return Environment.Development
  } else {
    return Environment.Production
  }
}

export const coreOptions: MilochauCoreOptions = {
  application: {
    name: 'Contacts',
    contact: 'Antoine Milochau',
    logoUrl,
    navigation: {
      items: navigationItems,
    },
    header: {
      onTitleClick: router => router.push({ name: 'Home' }),
    },
    isProduction: getCurrentEnvironment() === Environment.Production,
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
