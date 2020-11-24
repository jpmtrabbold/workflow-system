import { messageActions } from "./message-actions"
import { MessageOptions } from "./MessageDialog/MessageDialog";
type Without<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>

interface messageYesNoProps extends Without<MessageOptions, 'actions' | 'variant'> {
    
}
export async function messageYesNo({ title, content, ...other } : messageYesNoProps): Promise<boolean> {

    const answer = await messageActions({
        title: title, content: content, actions: [
            {
                name: 'Yes',
                color: 'primary'
            },
            {
                name: 'No',
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