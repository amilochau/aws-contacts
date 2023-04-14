import { defaultEnv, envConfig, getCurrentEnv } from "../data/config"

export function getConfig(key: string): string {
  const host = window.location.host
  const subdomain = host.split('.')[0]
  const currentEnv = getCurrentEnv(host, subdomain)

  return import.meta.env[key] ?? envConfig[currentEnv][key] ?? envConfig[defaultEnv][key]
}
