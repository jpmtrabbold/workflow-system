import React from 'react'
import MainPage from './features/main-page/MainPage'

import MomentUtils from '@date-io/moment'
import MuiPickersUtilsProvider from '@material-ui/pickers/MuiPickersUtilsProvider'
import { setCacheKey } from 'shared-do-not-touch/utils/cache'
import { MuiThemeProvider } from '@material-ui/core/styles'
import { BrowserRouter, Route, Switch } from "react-router-dom"

import 'App.css'
import { withAdalLoginApi, authContext } from 'clients/azure-authentication'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog'
import { LoadingModal } from 'shared-do-not-touch/material-ui-modals'
import { DealDirectWorkflowAction } from 'features/deals/entity-actions/direct-workflow-action/DealDirectWorkflowAction'
import { observer } from 'mobx-react-lite'
import { GlobalStore, GlobalStoreContext } from 'features/shared/stores/GlobalStore'
import { CssBaseline } from '@material-ui/core'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'


setCacheKey(process.env.REACT_APP_CACHE_KEY as string)

const AuthenticatedMainPage = withAdalLoginApi(MainPage,
   () => <LoadingModal title="Logging in..." visible={true} />,
   (error) => <><MessageDialog title='Login Error' content={JSON.stringify(error)} actions={[{ name: "Sign Out", callback: () => authContext.logOut() }]} /></>)

const App = observer(() => {
   const globalStore = useStore(() => new GlobalStore())

   return (
      <GlobalStoreContext.Provider value={globalStore}>
         <MuiThemeProvider theme={globalStore.theme}>
            <CssBaseline />
            <MuiPickersUtilsProvider utils={MomentUtils}>
               <BrowserRouter>
                  <Switch>
                     <Route path={'/deal-workflow-action/:dealId'} exact={false}>
                        <DealDirectWorkflowAction />
                     </Route>
                     <Route>
                        <AuthenticatedMainPage />
                     </Route>
                  </Switch>
               </BrowserRouter>
            </MuiPickersUtilsProvider>
         </MuiThemeProvider>
      </GlobalStoreContext.Provider>
   )
})

export default App