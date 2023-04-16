export class MessagesCreateRequest {
  content: MessageContent = new MessageContent()
}

export class MessagesDetailsResponse {
  id: string = '';
  creation: string = '';
  ttl?: string;
  userId?: string;
  status: MessageStatus = MessageStatus.New;
  content: MessageContent = new MessageContent()
  trackings: MessageTracking[] = [];
}

export class MessageContent {
  senderEmail: string = '';
  senderName: string = '';
  message: string = '';
  culture: string = '';
}

export class MessageTracking {
  creation: string = '';
  userId?: string;
  type: MessageTrackingType = MessageTrackingType.NotSet;
}

export enum MessageStatus {
  New = 0,
  InProgress = 1,
  Closed = 2,
}

export enum MessageTrackingType {
  NotSet = 0,
  Create = 1,
  StatusToNew = 2,
  StatusToInProgress = 3,
  StatusToClosed = 4,
}
