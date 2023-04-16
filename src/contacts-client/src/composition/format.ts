import { mdiAccountClock, mdiAlertCircle, mdiArchive, mdiMessagePlusOutline, mdiMessageProcessingOutline, mdiMessageCheckOutline, mdiMessageArrowLeftOutline, mdiMessageQuestionOutline } from '@mdi/js'
import { MessageStatus, MessageTrackingType } from "@/types/messages"
import { useI18n } from 'vue-i18n'
import type { FormattedData } from "@amilochau/core-vue3"

export function useFormat() {
  const { t, mergeLocaleMessage } = useI18n()

  mergeLocaleMessage('en', {
    messageStatuses: {
      new: "New",
      inProgress: "In progress",
      closed: "Closed"
    },
    messageTrackingTypes: {
      create: 'Create',
      StatusToNew: 'Status changed to new',
      StatusToInProgress: 'Status changed to in progress',
      StatusToClosed: 'Status changed to closed',
      notSet: '',
    }
  })
  mergeLocaleMessage('fr', {
    messageStatuses: {
      new: "Nouveau",
      inProgress: "En cours",
      closed: "Fermé"
    },
    messageTrackingTypes: {
      create: 'Création',
      StatusToNew: 'Statut modifié par nouveau',
      StatusToInProgress: 'Statut modifié par en cours',
      StatusToClosed: 'Statut modifié par fermé',
      notSet: '',
    }
  })

  return {
    formatMessageStatus: (status: MessageStatus): FormattedData<MessageStatus> => {
      switch (status) {
        case MessageStatus.New: {
          return {
            value: MessageStatus.New,
            title: t('messageStatuses.new'),
            icon: mdiAlertCircle,
            color: 'info',
          }
        }
        case MessageStatus.InProgress: {
          return {
            value: MessageStatus.InProgress,
            title: t('messageStatuses.inProgress'),
            icon: mdiAccountClock,
            color: 'warning',
          }
        }
        case MessageStatus.Closed: {
          return {
            value: MessageStatus.Closed,
            title: t('messageStatuses.closed'),
            icon: mdiArchive,
            color: 'success',
          }
        }
      }
    },
    formatMessageTrackingType: (messageTrackingType: MessageTrackingType): FormattedData<MessageTrackingType> => {
      switch (messageTrackingType) {
        case MessageTrackingType.Create: {
          return {
            value: MessageTrackingType.Create,
            title: t('messageTrackingTypes.create'),
            icon: mdiMessagePlusOutline,
            color: 'info',
          }
        }
        case MessageTrackingType.StatusToNew: {
          return {
            value: MessageTrackingType.StatusToNew,
            title: t('messageTrackingTypes.statusToNew'),
            icon: mdiMessageArrowLeftOutline,
            color: 'warning',
          }
        }
        case MessageTrackingType.StatusToInProgress: {
          return {
            value: MessageTrackingType.StatusToInProgress,
            title: t('messageTrackingTypes.statusToInProgress'),
            icon: mdiMessageProcessingOutline,
            color: 'warning',
          }
        }
        case MessageTrackingType.StatusToClosed: {
          return {
            value: MessageTrackingType.StatusToClosed,
            title: t('messageTrackingTypes.statusToClosed'),
            icon: mdiMessageCheckOutline,
            color: 'success',
          }
        }
        default:
          return {
            value: MessageTrackingType.NotSet,
            title: t('messageTrackingTypes.notSet'),
            icon: mdiMessageQuestionOutline,
            color: 'secondary',
          }
      }
    }
  }
}
