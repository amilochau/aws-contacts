import { Environment, defaultEnv, envConfig, getCurrentEnv } from "../data/config"

export function getCurrentEnvironment() : Environment {
  const host = window.location.host
  const subdomain = host.split('.')[0]
  return getCurrentEnv(host, subdomain)
}

export function getConfig(key: string): string {
  const currentEnv = getCurrentEnvironment()
  return import.meta.env[key] ?? envConfig[currentEnv][key] ?? envConfig[defaultEnv][key]
}
