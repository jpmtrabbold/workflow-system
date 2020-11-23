import { messageActions } from "./message-actions";
import { MessageOptions } from "./MessageDialog/MessageDialog";

type Without<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>

interface messageErrorProps extends Without<MessageOptions, 'actions' | 'variant'> {
    
}
export async function messageError({ title, content, ...other } : messageErrorProps) {
    if (title === undefined || title === null) {
        title = "Error"
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