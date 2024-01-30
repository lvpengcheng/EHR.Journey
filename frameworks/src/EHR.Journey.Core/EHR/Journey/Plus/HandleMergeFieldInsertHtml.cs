using Aspose.Words.MailMerging;
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Journey.Core
{
    public class HandleMergeFieldInsertHtml : IFieldMergingCallback
    {
        /// <summary>
        /// Called when a mail merge merges data into a MERGEFIELD.
        /// </summary>
        void IFieldMergingCallback.FieldMerging(FieldMergingArgs args)
        {
            if (args.DocumentFieldName.StartsWith("html_"))
            {
                // Add parsed HTML data to the document's body.
                DocumentBuilder builder = new DocumentBuilder(args.Document);
                builder.MoveToMergeField(args.DocumentFieldName);
                builder.InsertHtml((string)args.FieldValue);

                // Since we have already inserted the merged content manually,
                // we will not need to respond to this event by returning content via the "Text" property. 
                args.Text = string.Empty;
            }
        }

        void IFieldMergingCallback.ImageFieldMerging(ImageFieldMergingArgs args)
        {
            // Do nothing.
        }
    }
}
