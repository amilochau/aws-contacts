import type { MessagesCreateRequest, MessagesDetailsResponse } from "@/types/messages";
import { useApi, type IDefaultCreateResponse, AuthPolicy } from "@amilochau/core-vue3";

export function useMessagesAnonymousApi() {

  const api = useApi('/a/messages')

  const create = async (request: MessagesCreateRequest) => {
    const response = await api.postHttp<MessagesCreateRequest>('', request, { redirect404: false, authPolicy: AuthPolicy.SendRequestsAsAnonymous })
    return await response.json() as IDefaultCreateResponse
  }

  const getById = async (messageId: string) => {
    const response = await api.getHttp(`/${messageId}`, { redirect404: true, authPolicy: AuthPolicy.SendRequestsAsAnonymous })
    return await response.json() as MessagesDetailsResponse
  }

  return {
    create,
    getById,
  }
}
