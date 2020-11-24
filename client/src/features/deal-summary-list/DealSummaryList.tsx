import React from 'react'
import { observer } from 'mobx-react-lite'
import { DealSummaryListStore } from './DealSummaryListStore'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import { dealSummaryListGridDefinition } from './deal-summary-list-grid-definition'
import Grid from '@material-ui/core/Grid'
import { InputProps } from 'shared-do-not-touch/input-props'
import { KeyboardDatePicker } from '@material-ui/pickers/DatePicker'
import { makeStyles, useTheme } from '@material-ui/core/styles'
import Button from '@material-ui/core/Button'
import Divider from '@material-ui/core/Divider'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'
import { Tooltip } from '@material-ui/core'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const useStyles = makeStyles(theme => ({
    datePicker: {
        width: theme.spacing(22)
    },
    grid: {
        padding: theme.spacing(1, 1, 3, 3),
        width: '100%',
        display: 'flex', alignItems: 'center',
    },
    paper: {
        margin: theme.spacing(1, 1, 1, 1),
    }
}))

export const DealSummaryList = observer(() => {
    const classes = useStyles()
    const store = useStore(() => new DealSummaryListStore())
    const windowSize = useWindowSize()
    const theme = useTheme()

    return <>
        <CustomTable
            title="Deal Summary List"
            rows={store.rows}
            columns={dealSummaryListGridDefinition}
            maxHeight={windowSize.height - theme.spacing(10)}
            minHeight={windowSize.height - theme.spacing(10)}
            setStoreReference={store.setTableStoreReference}
            renderBetweenToolbarAndTable={(
                <>
                    <Grid className={classes.grid} container spacing={3}>
                        <Grid item>
                            <InputProps stateObject={store} propertyName='startCreationDate' config={{ elementValueForUndefinedOrNull: () => null }}>
                                <KeyboardDatePicker label="Start Creation Date" autoOk format="DD/MM/YYYY"
                                    className={classes.datePicker}
                                    value={null} onChange={store.nullChange}
                                />
                            </InputProps>
                        </Grid>
                        <Grid item>
                            <InputProps stateObject={store} propertyName='endCreationDate' config={{ elementValueForUndefinedOrNull: () => null }}>
                                <KeyboardDatePicker label="End Creation Date" autoOk format="DD/MM/YYYY"
                                    className={classes.datePicker}
                                    value={null} onChange={store.nullChange}
                                />
                            </InputProps>
                        </Grid>
                        <Grid item>
                            <Button variant='contained' color='primary' onClick={store.dataFetch}>
                                Search
                            </Button>
                        </Grid>
                        <Tooltip title="Generates the PDF with the grid data">
                            <Grid item>
                                <Button variant='contained' color='primary' onClick={store.generatePDF} disabled={store.rows.length === 0}>
                                    Generate PDF
                            </Button>
                            </Grid>
                        </Tooltip>
                    </Grid>
                    <Divider />
                </>
            )}
        />
    </>
})