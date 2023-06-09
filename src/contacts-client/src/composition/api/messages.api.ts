import type { MessagesCreateRequest, MessagesDetailsResponse } from "@/types/messages";
import { type IDefaultCreateResponse } from "@amilochau/core-vue3";
import { useApi } from "@amilochau/core-vue3-auth";

export const useMessagesApi = () => {

  const api = useApi('/messages')

  const create = async (request: MessagesCreateRequest) => {
    const response = await api.postHttp<MessagesCreateRequest>('', request, { redirect404: false })
    return await response.json() as IDefaultCreateResponse
  }

  const getById = async (messageId: string) => {
    const response = await api.getHttp(`/${messageId}`, { redirect404: true })
    return await response.json() as MessagesDetailsResponse
  }

  return {
    create,
    getById,
  }
}
