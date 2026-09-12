export type NotificationType = 'success' | 'error';

export interface Notification {
  message: string;
  type: NotificationType;
}