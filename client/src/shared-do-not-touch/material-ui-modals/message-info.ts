import { messageActions } from "./message-actions";
import { MessageOptions } from "./MessageDialog/MessageDialog";

type Without<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>

interface messageInfoProps extends Without<MessageOptions, 'actions' | 'variant'> {
    
}
export async function messageInfo({ title = "Info", content, ...other } : messageInfoProps) {
    if (title === undefined || title === null) {
        title = "Info"
    }

    return messageActions({
        title: title, content: content, actions: [
            {
                name: 'Ok',
                color: 'primary'
            },
        ],
        ...other
    })
}