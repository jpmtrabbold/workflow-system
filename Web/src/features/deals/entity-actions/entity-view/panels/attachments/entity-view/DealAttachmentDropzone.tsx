import React from 'react'
import makeStyles from "@material-ui/styles/makeStyles"
import { DropzoneArea } from "material-ui-dropzone"
import { Theme } from "@material-ui/core/styles"
import { observer } from "mobx-react-lite"
import { dealAttachmentTypes } from './attachmentTypes'

interface DealAttachmentDropzoneProps {
    handleFiles: (files: File[]) => any
}

const useStyles = makeStyles((theme: Theme) => ({
    dropzoneParagraph: {
        marginLeft: theme.spacing(4),
        marginRight: theme.spacing(4),
    },
}))

const DealAttachmentDropzone = (props: DealAttachmentDropzoneProps) => {
    const classes = useStyles()

    return (
        <DropzoneArea
            onChange={props.handleFiles}
            acceptedFiles={dealAttachmentTypes}
            maxFileSize={50 * 1024 * 1024}
            filesLimit={1}
            dropzoneText='Drop your file here or click to explore'
            dropzoneParagraphClass={classes.dropzoneParagraph}
            showPreviews={false}
            showPreviewsInDropzone={false}
            clearOnUnmount={true}
        />
    )
}

export default observer(DealAttachmentDropzone)