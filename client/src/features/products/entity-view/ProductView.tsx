// ** Common
import React, { useContext } from "react"
import { observer } from "mobx-react-lite"

import Button from "@material-ui/core/Button"
import Dialog from "@material-ui/core/Dialog"
import DialogActions from "@material-ui/core/DialogActions"
import DialogContent from "@material-ui/core/DialogContent"
import DialogTitle from "@material-ui/core/DialogTitle"
import Grid from "@material-ui/core/Grid"
import makeStyles from "@material-ui/core/styles/makeStyles"
import TextField from "@material-ui/core/TextField"
import useInitialMount from "shared-do-not-touch/hooks/useInitialMount"
import { ProductViewStoreContext } from "./ProductViewStore"
import DealCategoryViewLoader from "features/deal-categories/entity-view/DealCategoryViewLoader"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal } from "shared-do-not-touch/material-ui-modals"
import { SelectWithLabel } from "shared-do-not-touch/material-ui-select-with-label"
import { CheckBoxWithLabel } from "shared-do-not-touch/material-ui-checkbox-with-label"

const useStyles = makeStyles(theme => ({
   grid: {
      padLeft: theme.spacing(10),
      padRight: theme.spacing(10),
      justifyContent: "space-evenly"
   },
   container: {
      display: "flex"
   },
   dialog: {
      flexGrow: 1
   },
   clearButtonContainer: {
      display: "flex",
      alignItems: "center"
   }
}))

interface ProductViewProps {
   entityId?: number
   entityName?: string
   visible: boolean
   onEntityClose: () => any
   readOnly?: boolean
}

const ProductView = (props: ProductViewProps) => {
   const classes = useStyles()
   const store = useContext(ProductViewStoreContext)

   useInitialMount(() => {
      onLoad()
   })

   async function onLoad() {
      if (props.entityId) {
         if (!(await store.fetchData(props.entityId))) {
            return props.onEntityClose()
         }
      } else {
         if (!(await store.setupNewProduct())) return props.onEntityClose()
      }
      store.loaded = true
   }

   function handleClose() {
      props.onEntityClose()
   }

   async function save() {
      if (await store.saveProduct()) {
         handleClose()
      }
   }

   const { product } = store
   const { visible } = props

   if (!store.loaded) {
      return (
          <LoadingModal
              title={
                  !props.entityName
                      ? "Loading..."
                      : `Loading ${props.entityName}`
              }
              visible={visible}
          />
      )
  }

   return (
      <>
         <Dialog
            open={visible}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
            maxWidth="sm"
            scroll="body"
            className={classes.dialog}
         >
            <DialogTitle id="form-dialog-title">
               {props.readOnly ? "View Product" : store.creation ? "Add Product" : "Edit Product"}
            </DialogTitle>
            <div className={classes.container}>
               <DialogContent className={classes.grid}>
                  <Grid container spacing={2} className={classes.grid}>
                     <Grid item sm={6} xs={12}>
                        <InputProps stateObject={product} propertyName="name">

                           <TextField
                              disabled={props.readOnly}
                              label="Name"
                              required
                              fullWidth
                              autoFocus
                           />
                        </InputProps>
                     </Grid>

                     <Grid item sm={6} xs={12}>
                        <InputProps stateObject={product} propertyName="dealCategoryId">

                           <SelectWithLabel
                              disabled={props.readOnly}
                              required
                              fullWidth
                              label="Deal Category"
                              lookups={store.dealCategories}
                              lookupView={(props) => <DealCategoryViewLoader {...props} />}
                           />
                        </InputProps>
                     </Grid>
                     <Grid item sm={6} xs={6}>
                        <InputProps stateObject={product} propertyName="active">
                           <CheckBoxWithLabel
                              disabled={props.readOnly}
                              label="Is Active?"
                           />
                        </InputProps>
                     </Grid>
                  </Grid>
               </DialogContent>
            </div>
            <DialogActions>
               {!!props.readOnly && (
                  <Button onClick={handleClose} title="Close" color="primary">
                     Close
            </Button>
               )}
               {!props.readOnly && (
                  <>
                     <Button onClick={handleClose} title="Cancel" color="primary">
                        Cancel
              </Button>
                     <Button onClick={save} title="Saves the current Product" color="primary">
                        Ok
              </Button>
                  </>
               )}
            </DialogActions>
         </Dialog>
      </>
   )
}
export default observer(ProductView)
