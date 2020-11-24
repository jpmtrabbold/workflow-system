import { messageActions } from "./message-actions"
import { MessageOptions } from "./MessageDialog/MessageDialog";

type Without<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>

interface messageNoYesProps extends Without<MessageOptions, 'actions' | 'variant'> {
    
}
export async function messageNoYes({ title, content, ...other } : messageNoYesProps): Promise<boolean> {

    const answer = await messageActions({
        title: title, content: content, actions: [
            {
                name: 'No',
                color: 'primary'
            },
            {
                name: 'Yes',
                color: 'primary'
            },
        ],
        ...other
    })

    if (!!answer && answer.name === 'Yes') {
        return true
    }

    return false
}
