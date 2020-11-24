import { authContext } from "../../../clients/azure-authentication"
import DealGridView from "../../deals/DealGridView"
import { createContext } from "react"
import UserGridView from "../../users/UserGridView"
import React from "react"
import { userClient } from "../../../clients/deal-system-rest-clients"
import { getCacheHandler } from "shared-do-not-touch/utils/cache"
import DealGridAssignedView from "../../deals/variants/DealGridAssignedView"
import CounterpartyGridView from "../../counterparties/CounterpartyGridView"
import DealCategoryGridView from "features/deal-categories/DealCategoryGridView"
import ProductGridView from "../../products/ProductGridView"
import DealTypeGridView from "../../deal-types/DealTypeGridView"

// https://material.io/resources/icons
import PersonIcon from '@material-ui/icons/Person'
import ListIcon from '@material-ui/icons/List'
import GroupIcon from '@material-ui/icons/Group'
import BusinessIcon from '@material-ui/icons/Business'
import ShoppingCartIcon from '@material-ui/icons/ShoppingCart'
import AttachMoneyIcon from '@material-ui/icons/AttachMoney'
import PlaylistAddIcon from '@material-ui/icons/PlaylistAdd'
import TimelineIcon from '@material-ui/icons/Timeline'
import ListAltIcon from '@material-ui/icons/ListAlt'
import SettingsIcon from '@material-ui/icons/Settings'
import DealItemFieldsetGridView from "../../deal-item-fieldset/DealItemFieldsetGridView"
import MemoryIcon from '@material-ui/icons/Memory'
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import { cloneObject } from "shared-do-not-touch/utils/utils"
import { UserFunctionalityReadDto, FunctionalityEnum, SubFunctionalityEnum } from "../../../clients/deal-system-client-definitions"
import { DealSummaryList } from "features/deal-summary-list/DealSummaryList"
import SalesForecastGridView from "features/sales-forecasts/SalesForecastGridView"
import createMuiTheme, { Theme } from "@material-ui/core/styles/createMuiTheme"
import { theme, createTheme } from "shared-do-not-touch/theme/theme"
import { setModalTheme } from "shared-do-not-touch/material-ui-modals"
import { ConfigurationGridView } from "features/configuration/ConfigurationGridView"

const functionalitiesCache = getCacheHandler<UserFunctionalityReadDto[]>(
    'DealSystemMenuCache',
    data => data.map(item => item.toJSON()),
    cacheData => cacheData.map((item: any) => UserFunctionalityReadDto.fromJS(item))
)
interface LocalUserConfig {
    darkTheme: boolean
}
const userConfigCache = getCacheHandler<LocalUserConfig>('DealSystemLocalUserConfig')

type FunctionalityChecker = (FunctionalityEnum | ((funcs: UserFunctionalityReadDto[]) => boolean))
interface IMenuItem {
    path?: string
    absolutePath?: string
    name: string
    exact: boolean
    landingPage: boolean;
    component: React.ComponentType<any>
    icon: React.ComponentType
    displaysInMenu: boolean
    functionality: FunctionalityChecker
}
export interface IMenuItemGroup {
    divisionName: string
    icon?: string
    routes: IMenuItem[]
}

const defaultMenuItems: IMenuItemGroup[] = [
    {
        divisionName: "Deals",
        routes: [
            {
                landingPage: false, functionality: FunctionalityEnum.Deals, exact: true, displaysInMenu: true,
                name: "All Deals", path: "/all-deals/",
                component: DealGridView,
                icon: ListIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.Deals, exact: true, displaysInMenu: false,
                name: "All Deals", path: "/all-deals/:dealId",
                component: DealGridView,
                icon: ListIcon
            },
            {
                landingPage: true, functionality: FunctionalityEnum.Deals, exact: true, displaysInMenu: true,
                name: "Assigned To Me", path: "/assigned-deals/",
                component: DealGridAssignedView,
                icon: PersonIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.DealSummaryList, exact: false, displaysInMenu: true,
                name: "Deal Summary List", path: "/deal-summary-list/",
                component: DealSummaryList,
                icon: ListAltIcon
            },
        ],
    },
    {
        divisionName: "Admin",
        routes: [
            {
                landingPage: false, functionality: FunctionalityEnum.Users, exact: false, displaysInMenu: true, name: "Users", path: "/users/",
                component: UserGridView,
                icon: GroupIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.Counterparties, exact: true, displaysInMenu: true,
                name: "Counterparties", path: "/counterparties/",
                component: CounterpartyGridView,
                icon: BusinessIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.Counterparties, exact: true, displaysInMenu: false,
                name: "Counterparties", path: "/counterparties/:id",
                component: CounterpartyGridView,
                icon: BusinessIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.DealCategories, exact: false, displaysInMenu: true,
                name: "Deal Categories", path: "/deal-categories/",
                component: DealCategoryGridView,
                icon: ShoppingCartIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.DealTypes, exact: false, displaysInMenu: true,
                name: "Deal Types", path: "/deal-types/",
                component: DealTypeGridView,
                icon: AttachMoneyIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.DealItemFieldsets, exact: false, displaysInMenu: true,
                name: "Deal Item Fieldsets", path: "/deal-item-fieldsets/",
                component: DealItemFieldsetGridView,
                icon: PlaylistAddIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.Products, exact: false, displaysInMenu: true,
                name: "Products", path: "/products/",
                component: ProductGridView,
                icon: MemoryIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.SalesForecasts, exact: false, displaysInMenu: true,
                name: "Sales Forecasts", path: "/sales-forecasts/",
                component: SalesForecastGridView,
                icon: TimelineIcon
            },
            {
                landingPage: false, functionality: FunctionalityEnum.Configuration, exact: false, displaysInMenu: true,
                name: "System Config.", path: "/configuration/",
                component: ConfigurationGridView,
                icon: SettingsIcon
            },
        ],
    },
    // {
    //     divisionName: "Integration",
    //     routes: [
    //         {
    //             landingPage: false, functionality: FunctionalityEnum.EmsIntegration, exact: true, displaysInMenu: true, name: "EMS Integration", path: "/integration/ems",
    //             component: IntegrationDashboardEms,
    //             icon: AccountTreeIcon
    //         },
    //         {
    //             landingPage: false, functionality: FunctionalityEnum.EmsIntegration, exact: true, displaysInMenu: false, name: "EMS Integration", path: "/integration/ems/:integrationRunId",
    //             component: IntegrationDashboardEms,
    //             icon: AccountTreeIcon
    //         },
    //         {
    //             landingPage: false, functionality: (funcs: UserFunctionalityReadDto[]) => {
    //                 return !!funcs.find(f => [
    //                     FunctionalityEnum.EmsIntegration,
    //                     FunctionalityEnum.AsxIntegration
    //                 ].includes(f.functionalityEnum))
    //             }, exact: true, displaysInMenu: true, name: "Scheduled Jobs", absolutePath: process.env.REACT_APP_API_BASE_URL + "/api/Login/HangfireLogin",
    //             component: () => null,
    //             icon: ScheduleIcon
    //         },
    //     ],
    // },
]

export interface FunctionalityHelper {
    permitted: boolean,
    functionality?: UserFunctionalityReadDto,
    hasSubFunctionality: (subFunctionalityEnum: SubFunctionalityEnum) => boolean
}

export class GlobalStore {

    constructor() {
        (window as any)['setInvalidLoginMessage'] = this.setInvalidLoginMessage

        this.setTheme(userConfigCache.get()?.darkTheme)
    }
    userPopperOpen = false
    openUserPopper = () => {
        this.userPopperOpen = true
    }
    closeUserPopper = () => {
        this.userPopperOpen = false
    }
    get appBarTitle() {
        return 'WorkflowSystem ' +
            ""//(process.env.REACT_APP_ENVIRONMENT_ID !== "prod" ? "(" + process.env.REACT_APP_ENVIRONMENT_NAME + ")" : "")
    }
    standardTheme = createMuiTheme()
    darkTheme = false
    theme: Theme = theme
    toggleTheme = () => {
        this.setTheme(!this.darkTheme)
        this.closeUserPopper()
    }
    setTheme = (dark = false) => {
        this.darkTheme = dark
        this.theme = createTheme(dark)
        setModalTheme(this.theme)
        userConfigCache.set({ darkTheme: dark })
    }

    getMenuItems = async () => {
        functionalitiesCache.withCache({
            getDataSilently: () => userClient.listFunctionalities(),
            getDataWithLoading: () => executeLoading("Loading menu...", () => userClient.listFunctionalities()),
            whatToDoWithData: async data => this.renderMenuItems(data),
        })
    }
    
    invalidLoginMessage?: string
    setInvalidLoginMessage = (message: string) => {
        this.invalidLoginMessage = message
    }

    menuItems: IMenuItemGroup[] = []
    userFunctionalities: UserFunctionalityReadDto[] = []

    /**
     * 
     * @param functionalityEnum the functionality you want
     * @returns a function that will return the functionalities and a method to check whether the user has access to a sub-functionality
     * of that functionality
     * @example
     * // usage:
     * const dealFunctionality = globalStore.functionality(FunctionalityEnum.Deals)
     * if (dealFunctionality.hasSubFunctionality(SubFunctionalityEnum.Edit)) {
     *    // do something that requires deal editing permission
     * }
     */
    functionality = (functionalityEnum: FunctionalityEnum): FunctionalityHelper => {
        const funcs = this.userFunctionalities
        const functionality = funcs.find(uf => uf.functionalityEnum === functionalityEnum)
        if (!!functionality) {
            return {
                permitted: true,
                functionality,
                hasSubFunctionality: (subFunctionalityEnum: SubFunctionalityEnum) => {
                    return !!(functionality.subFunctionalities.find(sf => sf.subFunctionalityEnum === subFunctionalityEnum))
                }
            }
        }
        return {
            permitted: true,
            hasSubFunctionality: () => false,
        }
    }

    private renderMenuItems = (userFunctionalities: UserFunctionalityReadDto[]) => {
        this.userFunctionalities = userFunctionalities
        let newMenuItems: IMenuItemGroup[] = []

        for (const defaultMenuItemGroup of defaultMenuItems) {
            const menuItemGroup = cloneObject(defaultMenuItemGroup)
            menuItemGroup.routes = []
            for (const defaultRoute of defaultMenuItemGroup.routes) {

                if (typeof (defaultRoute.functionality) === 'function') {
                    if (!defaultRoute.functionality(userFunctionalities)) {
                        continue
                    }
                } else {
                    if (!userFunctionalities.find(uf => uf.functionalityEnum === defaultRoute.functionality)) {
                        continue
                    }
                }

                if (defaultRoute.landingPage) {
                    const landing = cloneObject(defaultRoute)
                    landing.path = '/'
                    landing.exact = true
                    landing.displaysInMenu = false
                    landing.component = defaultRoute.component
                    landing.icon = defaultRoute.icon
                    menuItemGroup.routes.push(landing)
                }
                const menuItem = cloneObject(defaultRoute)
                menuItem.component = defaultRoute.component
                menuItem.icon = defaultRoute.icon
                menuItemGroup.routes.push(menuItem)
            }
            if (menuItemGroup.routes.length > 0) {
                newMenuItems.push(menuItemGroup)
            }
        }
        this.menuItems = newMenuItems
    }

    get username() {
        const cachedUser = authContext.getCachedUser()
        if (cachedUser)
            return cachedUser.profile.name
        else
            return "not logged in"
    }

    /** defines if the menu drawer is open */
    drawerOpen = false
}

export const GlobalStoreContext = createContext(new GlobalStore()) // this is to be accessed by the react components