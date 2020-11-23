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
import { DealCategoryViewStoreContext } from "./DealCategoryViewStore"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal } from "shared-do-not-touch/material-ui-modals"
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
	}
}))

interface DealCategoryViewProps {
	readOnly?: boolean
	entityId?: number
	entityName?: string
	visible: boolean
	onEntityClose: () => any
}

const DealCategoryView = (props: DealCategoryViewProps) => {
	const classes = useStyles()
	const store = useContext(DealCategoryViewStoreContext)

	useInitialMount(() => {
		onLoad()
	})

	async function onLoad() {
		if (props.entityId) {
			if (!(await store.fetchData(props.entityId))) {
				return props.onEntityClose()
			}
		} else {
			if (!(await store.setupNewDealCategory()))
				return props.onEntityClose()
		}
		store.loaded = true
	}

	function handleClose() {
		props.onEntityClose()
	}

	async function save() {
		if (await store.saveDealCategory()) {
			handleClose()
		}
	}

	const { dealCategory } = store
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
					{store.creation ? "Add Deal Category" : props.readOnly ? "View Deal Category" : "Edit Deal Category"}
				</DialogTitle>
				<div className={classes.container}>
					<DialogContent className={classes.grid}>
						<LoadingModal title="Saving Deal Category..." visible={store.isSaving} />
						<Grid container spacing={2} className={classes.grid}>
							<Grid item sm={6} xs={12}>
								<InputProps stateObject={dealCategory} propertyName="name">

									<TextField
										label="Name"
										required
										fullWidth
										autoFocus
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>

							<Grid item sm={6} xs={12}>
								<InputProps stateObject={dealCategory} propertyName="unitOfMeasure">

									<TextField
										label="Unit of Measure"
										fullWidth
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
							<Grid item sm={6} xs={6}>
								<InputProps stateObject={dealCategory} propertyName="active">
									<CheckBoxWithLabel
										label="Is Active?"
										disabled={props.readOnly}
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
							<Button
								onClick={save}
								title="Saves the current Deal Category"
								color="primary"
							>
								Ok
          					</Button>
						</>
					)}
				</DialogActions>
			</Dialog>
		</>
	)
}

export default observer(DealCategoryView)
