
export interface ModalStateParams {
    onOpen?: () => boolean | void,
    onClose?: () => boolean | void,
}
export default class ModalState {
    /**
     *
     */
    constructor(params: ModalStateParams = {}) {
        this.params = params
    }
    params: ModalStateParams
    visible = false
    open = () => {
        const should = this.params.onOpen && this.params.onOpen()
        if (typeof (should) === 'undefined' || should) {
            this.visible = true
        }
    }
    close = () => {
        const should = this.params.onClose && this.params.onClose()
        if (typeof (should) === 'undefined' || should) {
            this.visible = false
        }
        
    }
}