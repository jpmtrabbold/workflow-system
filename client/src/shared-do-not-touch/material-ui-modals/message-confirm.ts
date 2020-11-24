import { messageActions } from "."
import { MessageOptions } from "./MessageDialog/MessageDialog";

type Without<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>

interface messageConfirmProps extends Without<MessageOptions, 'actions' | 'variant'> {
    preCallbackValidation?: () => Promise<boolean>
}
export async function messageConfirm({ title, content, preCallbackValidation, ...other }: messageConfirmProps): Promise<boolean> {

    const answer = await messageActions({
        title: title,
        content: content,
        actions: [
            {
                name: 'Ok',
                color: 'primary',
                preCallbackValidation: preCallbackValidation,
            },
            {
                name: 'Cancel',
                color: 'primary'
            },
        ],
        ...other
    })

    if (!!answer && answer.name === 'Ok') {
        return true
    }

    return false
}