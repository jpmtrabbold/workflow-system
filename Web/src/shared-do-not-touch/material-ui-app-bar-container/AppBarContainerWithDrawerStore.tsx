import DisposableReactionsStore from "shared-do-not-touch/mobx-utils/DisposableReactionsStore";
import { AppBarContainerWithDrawerProps } from "./AppBarContainerWithDrawer";

type ParamsProps = AppBarContainerWithDrawerProps &  {
    bigScreen: boolean
    initialDrawerOpen?: boolean
}

export const appBarContainerWithDrawerStoreDisposer = (store: AppBarContainerWithDrawerStore, diposer: DisposableReactionsStore) => {
    diposer.registerAutorun(() => {
        if (store.sp.initialDrawerOpen !== undefined) {
            store.setOpen(store.sp.initialDrawerOpen)
        }
    })
    diposer.registerAutorun(() => {
        if (store.sp.bigScreen) {
            store.setBigScreen(true)
        } else {
            store.setBigScreen(false)
        }

    })

}
export class AppBarContainerWithDrawerStore {
   
    constructor(sp: ParamsProps) {
        this.sp = sp
        this.permanent = sp.bigScreen    
        this.sp.setStore && this.sp.setStore(this)

    }
    sp: ParamsProps

    drawer: HTMLDivElement | null = null
    drawerWidth?: number
    
    setDrawer = (drawer: HTMLDivElement | null) => {
        if (drawer) {
            this.drawer = drawer
            this.drawerWidth = this.drawer.clientWidth
            window.removeEventListener('resize', this.setDrawerWidth)
            window.addEventListener('resize', this.setDrawerWidth)
        } else {
            this.drawer = null
            window.removeEventListener('resize', this.setDrawerWidth)
        }
    }

    setDrawerWidth = () => {
        setTimeout(() => {
            this.drawerWidth = this.drawer?.clientWidth ?? undefined   
        });
    }
    
    open = false
    permanent = true

    setBigScreen = (big: boolean) => {
        this.setOpen(big)
        this.permanent = big
    }

    setOpen = (open: boolean) => this.open = open
    onClose = () => this.open = false
    drawerToggle = () => this.open = !this.open
    closeIfTemporary = () => !this.permanent && this.setOpen(false)

    get drawerStyle(): React.CSSProperties {
        return {
            minWidth: 250
        }
    }

    get childrenDivStyle(): React.CSSProperties | undefined {
        if (this.permanent && this.open && this.drawerWidth) {
            return { marginLeft: this.drawerWidth }
        }
        return undefined
    }
}