// ** Common
import React, { useCallback } from "react"
import { observer } from "mobx-react-lite"

import Button from "@material-ui/core/Button"
import Dialog from "@material-ui/core/Dialog"
import DialogActions from "@material-ui/core/DialogActions"
import DialogContent from "@material-ui/core/DialogContent"
import DialogTitle from "@material-ui/core/DialogTitle"
import Grid from "@material-ui/core/Grid"
import makeStyles from "@material-ui/core/styles/makeStyles"
import { CounterpartyViewStore } from "./CounterpartyViewStore"
import TextField from "@material-ui/core/TextField"
import Table from "@material-ui/core/Table"
import TableHead from "@material-ui/core/TableHead"
import TableRow from "@material-ui/core/TableRow"
import TableCell from "@material-ui/core/TableCell"
import TableBody from "@material-ui/core/TableBody"
import useInitialMount from "shared-do-not-touch/hooks/useInitialMount"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal } from "shared-do-not-touch/material-ui-modals"
import ViewLoaderProps from "shared-do-not-touch/interfaces/ViewLoaderProps"
import { CounterpartyDealCategoryView } from "./CounterpartyDealCategoryView"
import { AutoCompleteField } from "shared-do-not-touch/material-ui-auto-complete"
import { KeyboardDatePicker } from "@material-ui/pickers/DatePicker"
import { YesNoSelectWithLabel } from "shared-do-not-touch/material-ui-select-with-label"
import useStore from "shared-do-not-touch/mobx-utils/useStore"

const useStyles = makeStyles(theme => ({
    grid: {
        padLeft: theme.spacing(10),
        padRight: theme.spacing(10),
        justifyContent: "space-evenly"
    },
    container: {
        display: "flex"
    }
}))

const CounterpartyView = (props: ViewLoaderProps) => {
    const classes = useStyles()
    const store = useStore(source => new CounterpartyViewStore(source), props)
    const nullChange = useCallback(() => null, [])

    useInitialMount(() => {
        store.onLoad()
    })

    const { entity } = store
    const { visible } = props

    if (!store.loaded) {
        return <LoadingModal title={!props.entityName ? "Loading..." : `Loading ${props.entityName}`} visible={visible} />
    }

    return (
        <>
            <Dialog
                open={visible}
                onClose={store.handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="sm"
                scroll="body"
            >
                <DialogTitle id="form-dialog-title">
                    {store.creation ? "Add Counterparty" : props.readOnly ? "View Counterparty" : "Edit Counterparty"}
                </DialogTitle>
                <div className={classes.container}>
                    <DialogContent className={classes.grid}>
                        <Grid container spacing={2} className={classes.grid}>
                            <Grid item sm={8} xs={12}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="name">
                                    <TextField
                                        label="Counterparty Name"
                                        required
                                        fullWidth
                                        autoFocus
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="code">

                                    <TextField
                                        label="Counterparty Code"
                                        required
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="countryId">
                                    <AutoCompleteField
                                        dataSource={store.countries}
                                        label="Country"
                                        disabled={props.readOnly}
                                        fullWidth
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="companyNumber" config={{ valueRestrictors: [store.spaceAndNumbersOnly] }}>
                                    <TextField
                                        label="Company Number"
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="businessNumber" config={{ valueRestrictors: [store.spaceAndNumbersOnly] }}>
                                    <TextField
                                        label="Business Number"
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="nzemParticipant" config={{ isCheckbox: false }} onValueChange={store.onNzemChange}>
                                    <YesNoSelectWithLabel
                                        label="NZEM Participant?"
                                        fullWidth
                                        disabled={props.readOnly}
                                        hasNotSelectedOption
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="nzemParticipantId">
                                    <TextField
                                        label="NZEM Participant ID"
                                        fullWidth
                                        disabled={props.readOnly || store.NzemIdDisabled}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps
                                    errorHandler={store.errorHandler}
                                    stateObject={entity} propertyName="exposureLimit"
                                    variant='numeric'
                                    config={{ maxDecimalPlaces: 2 }}
                                >
                                    <TextField
                                        label="Exposure Limit"
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="expiryDate" config={{ elementValueForUndefinedOrNull: () => null }}>
                                    <KeyboardDatePicker
                                        autoOk
                                        disabled={props.readOnly}
                                        label="Expiry Date"
                                        fullWidth
                                        value={null}
                                        onChange={nullChange}
                                        format="DD/MM/YYYY"
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="approvalDate" config={{ elementValueForUndefinedOrNull: () => null }}>
                                    <KeyboardDatePicker
                                        autoOk
                                        disabled={props.readOnly}
                                        label="Approval Date"
                                        fullWidth
                                        value={null}
                                        onChange={nullChange}
                                        format="DD/MM/YYYY"
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item sm={4} xs={6}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="active" config={{ isCheckbox: false }}>
                                    <YesNoSelectWithLabel
                                        label="Is Active?"
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item xs={12}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="conditions">
                                    <TextField
                                        label="Conditions"
                                        multiline
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>
                            <Grid item xs={12}>
                                <InputProps errorHandler={store.errorHandler} stateObject={entity} propertyName="securityHeld">
                                    <TextField
                                        label="Security Held"
                                        multiline
                                        fullWidth
                                        disabled={props.readOnly}
                                    />
                                </InputProps>
                            </Grid>

                        </Grid>
                        <br />

                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell style={{ width: '30px' }} align='center'>Selected</TableCell>
                                    <TableCell>Deal Category</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {store.dealCategories.map(row => <CounterpartyDealCategoryView
                                    key={row.id}
                                    row={row}
                                    readOnly={props.readOnly}
                                    dealCategories={store.entity.dealCategories}
                                    toggleDealCategorySelection={store.toggleDealCategorySelection}
                                />)}
                            </TableBody>
                        </Table>
                    </DialogContent>
                </div>
                <DialogActions>
                    {!!props.readOnly && (
                        <Button onClick={store.handleClose} title="Close" color="primary">
                            Close
                        </Button>
                    )}

                    {!props.readOnly && (
                        <>
                            <Button onClick={store.handleClose} title="Cancel" color="primary">
                                Cancel
                            </Button>
                            <Button
                                onClick={store.saveAndClose}
                                title="Saves the current counterparty"
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

export default observer(CounterpartyView)
