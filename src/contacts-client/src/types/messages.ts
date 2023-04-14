export class MessagesCreateRequest {
  content: MessageContent = new MessageContent()
}

export class MessageContent {
  senderEmail: string = '';
  senderName: string = '';
  message: string = '';
  culture: string = '';
}
