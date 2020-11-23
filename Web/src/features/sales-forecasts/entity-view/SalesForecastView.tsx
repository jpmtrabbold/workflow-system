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
import { SalesForecastViewStoreContext } from "./SalesForecastViewStore"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal } from "shared-do-not-touch/material-ui-modals"
import { KeyboardDatePicker } from "@material-ui/pickers/DatePicker"

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

interface SalesForecastViewProps {
	readOnly?: boolean
	entityId?: number
	entityName?: string
	visible: boolean
	onEntityClose: () => any
}

const SalesForecastView = (props: SalesForecastViewProps) => {
	const classes = useStyles()
	const store = useContext(SalesForecastViewStoreContext)

	useInitialMount(() => {
		onLoad()
	})

	async function onLoad() {
		if (props.entityId) {
			if (!(await store.fetchData(props.entityId))) {
				return props.onEntityClose()
			}
		} else {
			if (!(await store.setupNewSalesForecast()))
				return props.onEntityClose()
		}
		store.loaded = true
	}

	function handleClose() {
		props.onEntityClose()
	}

	async function save() {
		if (await store.saveSalesForecast()) {
			handleClose()
		}
	}

	const { salesForecast } = store
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
					{store.creation ? "Add Sales Forecast" : props.readOnly ? "View Sales Forecast" : "Edit Sales Forecast"}
				</DialogTitle>
				<div className={classes.container}>
					<DialogContent className={classes.grid}>
						<LoadingModal title="Saving Sales Forecast..." visible={store.isSaving} />
						<Grid container spacing={2} className={classes.grid}>
							<Grid item sm={6} xs={12}>
								<InputProps stateObject={salesForecast} propertyName="monthYear" config={{ elementValueForUndefinedOrNull: () => null }}>
									<KeyboardDatePicker
										autoOk
										disabled={props.readOnly}
										views={['month', 'year']}
										label="Month / Year"
										fullWidth
										value={null}
										onChange={store.nullChange}
									/>
								</InputProps>
							</Grid>
							<Grid item sm={6} xs={12}>
								<InputProps stateObject={salesForecast} propertyName="volume" variant={"numeric"}>
									<TextField
										label="Volume (MW)"
										fullWidth
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
								title="Saves the current Sales Forecast"
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

export default observer(SalesForecastView)
