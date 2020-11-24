import { messageActions } from "./message-actions";
import { MessageOptions } from "./MessageDialog/MessageDialog";

type Without<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>

interface messageWarningProps extends Without<MessageOptions, 'actions' | 'variant'> {
    
}
export async function messageWarning({ title = "Warning", content, ...other } : messageWarningProps) {
    if (title === undefined || title === null) {
        title = "Warning"
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