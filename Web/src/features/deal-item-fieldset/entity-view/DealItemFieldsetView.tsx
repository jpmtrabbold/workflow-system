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
import IconButton from "@material-ui/core/IconButton"
import useInitialMount from "shared-do-not-touch/hooks/useInitialMount"
import { DealItemFieldsetViewStoreContext } from "./DealItemFieldsetViewStore"
import Table from "@material-ui/core/Table"
import TableHead from "@material-ui/core/TableHead"
import TableRow from "@material-ui/core/TableRow"
import TableCell from "@material-ui/core/TableCell"
import TableBody from "@material-ui/core/TableBody"
import Checkbox from "@material-ui/core/Checkbox"
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown"
import ArrowDropUpIcon from "@material-ui/icons/ArrowDropUp"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal } from "shared-do-not-touch/material-ui-modals"

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

interface DealItemFieldsetViewProps {
	readOnly?: boolean
	entityId?: number
	entityName?: string
	visible: boolean
	onEntityClose: () => any
}

const DealItemFieldsetView = (props: DealItemFieldsetViewProps) => {
	const classes = useStyles()
	const store = useContext(DealItemFieldsetViewStoreContext)

	useInitialMount(() => {
		onLoad()
	})

	async function onLoad() {
		if (props.entityId) {
			if (!(await store.fetchData(props.entityId))) {
				return props.onEntityClose()
			}
		} else {
			if (!(await store.setupNewDealItemFieldset()))
				return props.onEntityClose()
		}
		store.loaded = true
	}

	function handleClose() {
		props.onEntityClose()
	}

	async function save() {
		if (await store.saveDealItemFieldset()) {
			handleClose()
		}
	}

	const { itemFieldset } = store
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
				maxWidth="md"
				scroll="body"
				className={classes.dialog}
			>
				<DialogTitle id="form-dialog-title">
					{store.creation ? "Add Deal Item Fieldset" : props.readOnly ? "View Deal Item Fieldset" : "Edit Deal Item Fieldset"}
				</DialogTitle>
				<div className={classes.container}>
					<DialogContent className={classes.grid}>
						<LoadingModal title="Saving Deal Item Fieldset..." visible={store.isSaving} />
						<Grid container spacing={2} className={classes.grid}>
							<Grid item xs={12}>
								<InputProps stateObject={itemFieldset} propertyName="name">

									<TextField
										label="Name"
										required
										fullWidth
										autoFocus
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={12}>
								<InputProps stateObject={itemFieldset} propertyName="description">

									<TextField
										label="Description"
										required
										fullWidth
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={12}>
								<Table size="small">
									<TableHead>
										<TableRow>
											<TableCell style={{ width: '30px' }} align='center'>Selected</TableCell>
											<TableCell>Name</TableCell>
											<TableCell>Execution?</TableCell>
											<TableCell>FieldName</TableCell>
											{!props.readOnly && (
												<TableCell style={{ width: '120px' }} align='center'>Order</TableCell>
											)}
										</TableRow>
									</TableHead>
									<TableBody >
										{store.itemFields.map((row, index) => (
											<TableRow key={row.field}>
												<TableCell style={{ width: '30px' }} align='center'>
													<InputProps stateObject={row} propertyName={'selected'}>
														<Checkbox
															disabled={props.readOnly}
														/>
													</InputProps>
												</TableCell>
												<TableCell>
													<InputProps stateObject={row} propertyName="name">
														<TextField
															fullWidth
															disabled={props.readOnly}
														/>
													</InputProps>
												</TableCell>
												<TableCell style={{ width: '30px' }} align='center'>
													{row.selected && (
														<InputProps stateObject={row} propertyName="execution">
															<Checkbox disabled={props.readOnly} />
														</InputProps>
													)}
												</TableCell>
												<TableCell>{row.field}</TableCell>
												{!props.readOnly && (
													<TableCell style={{ width: '150px' }} align='center'>
														<IconButton onClick={() => store.MoveFieldUp(index)}><ArrowDropUpIcon /> </IconButton>
														<IconButton onClick={() => store.MoveFieldDown(index)}><ArrowDropDownIcon /> </IconButton>
													</TableCell>
												)}
											</TableRow>
										))}
									</TableBody>
								</Table>
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
								title="Saves the current Deal Item Fieldset"
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

export default observer(DealItemFieldsetView)
