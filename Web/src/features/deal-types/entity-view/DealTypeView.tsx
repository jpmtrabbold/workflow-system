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
import { DealTypeViewStoreContext } from "./DealTypeViewStore"
import { positionLookup } from "../../shared/helpers/lookups"
import Table from "@material-ui/core/Table"
import TableHead from "@material-ui/core/TableHead"
import TableRow from "@material-ui/core/TableRow"
import TableCell from "@material-ui/core/TableCell"
import TableBody from "@material-ui/core/TableBody"
import Checkbox from "@material-ui/core/Checkbox"
import { LookupRequest } from "../../../clients/deal-system-client-definitions"
import DealItemFieldsetViewLoader from "../../deal-item-fieldset/entity-view/DealItemFieldsetViewLoader"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal } from "shared-do-not-touch/material-ui-modals"
import { CheckBoxWithLabel } from "shared-do-not-touch/material-ui-checkbox-with-label"
import { SelectWithLabel, YesNoSelectWithLabel } from "shared-do-not-touch/material-ui-select-with-label"


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

interface DealTypeViewProps {
	readOnly?: boolean
	entityId?: number
	entityName?: string
	visible: boolean
	onEntityClose: () => any
}

const DealTypeView = (props: DealTypeViewProps) => {
	const classes = useStyles()
	const store = useContext(DealTypeViewStoreContext)

	useInitialMount(() => {
		onLoad()
	})

	async function onLoad() {
		if (props.entityId) {
			if (!(await store.fetchData(props.entityId))) {
				return props.onEntityClose()
			}
		} else {
			if (!(await store.setupNewDealType()))
				return props.onEntityClose()
		}
		store.loaded = true
	}

	function handleClose() {
		props.onEntityClose()
	}

	async function save() {
		if (await store.saveDealType()) {
			handleClose()
		}
	}

	const { dealType } = store
	const { visible } = props

	if (!store.loaded)
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

	function onDealCategorySelected(dealCategory: LookupRequest) {
		const index = store.dealType.dealCategories.findIndex(pt => pt === dealCategory.id)
		if (index >= 0)
			store.dealType.dealCategories.splice(index, 1)
		else
			store.dealType.dealCategories.push(dealCategory.id)
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
					{store.creation ? "Add Deal Type" : props.readOnly ? "View Deal Type" : "Edit Deal Type"}
				</DialogTitle>
				<div className={classes.container}>
					<DialogContent className={classes.grid}>
						<Grid container spacing={2} className={classes.grid}>
							<Grid item xs={6}>
								<InputProps stateObject={dealType} propertyName="name">
									<TextField
										label="Name"
										required
										fullWidth
										autoFocus
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={6}>
								<InputProps stateObject={dealType} propertyName="unitOfMeasure">
									<TextField
										label="Unit of Measure"
										required
										fullWidth
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={6}>
								<InputProps stateObject={dealType} propertyName="position">
									<SelectWithLabel
										label="Default Position"
										required
										fullWidth
										disabled={props.readOnly}
										lookups={positionLookup}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={6}>
								<InputProps stateObject={dealType} propertyName="forcePosition" config={{ isCheckbox: false }}>
									<YesNoSelectWithLabel
										label="Force Position?"
										fullWidth
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>

							<Grid item xs={12}>
								<InputProps stateObject={dealType} propertyName="dealItemFieldsetId">
									<SelectWithLabel
										label="Deal Item Fieldset"
										required
										fullWidth
										disabled={props.readOnly}
										lookups={store.itemFieldsetLookups}
										lookupView={(props) => (
											<DealItemFieldsetViewLoader {...props} />
										)}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={12}>
								<InputProps stateObject={dealType} propertyName="workflowSetId">
									<SelectWithLabel
										label="Workflow Set"
										required
										fullWidth
										disabled={props.readOnly}
										lookups={store.workflowSetLookups}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={4}>
								<InputProps stateObject={dealType} propertyName="hasDelegatedAuthority">
									<CheckBoxWithLabel
										label="Supports Delegated Authority?"
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
							<Grid item xs={4}>
								<InputProps stateObject={dealType} propertyName="active">
									<CheckBoxWithLabel
										label="Is Active?"
										disabled={props.readOnly}
									/>
								</InputProps>
							</Grid>
						</Grid>
						<Table>
							<TableHead>
								<TableRow>
									<TableCell style={{ width: '30px' }} align='center'>Selected</TableCell>
									<TableCell>Deal Category</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{store.dealCategories.map(row => (
									<TableRow key={row.id}>
										<TableCell style={{ width: '30px' }} align='center'>
											<Checkbox
												disabled={props.readOnly}
												checked={
													!!store.dealType.dealCategories.find(
														pt => pt === row.id
													)
												}
												onChange={() => onDealCategorySelected(row)}
											/>
										</TableCell>
										<TableCell>{row.name}</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
						<LoadingModal title="Saving Deal Type..." visible={store.isSaving} />
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
								title="Saves the current Deal Type"
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

export default observer(DealTypeView)
