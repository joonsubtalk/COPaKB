using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class interactome_drosophila_mitochondria : System.Web.UI.Page
{
    string str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        str = Request.QueryString["iid"];
        if (str != null)
        {
            this.contentJS.Text = string.Format(@"<script> function startInteract(){{
nodeName = '{0}';
copaInteractome.copaProteinLink = 'http://www.heartproteome.org/copa/ProteinInfo.aspx?QType=Protein%20ID&QValue=';
copaInteractome.selectedProtein = nodeName;
copaInteractome.go_processes.highlightEntries(new Array(nodeName), 'highlighted');
copaInteractome.go_processes.unhighlightInputs();
copaInteractome.go_processes.highlightInputsByEntries(new Array(nodeName));
copaInteractome.reactome.highlightEntries(new Array(nodeName), 'highlighted');
copaInteractome.reactome.unhighlightInputs();
copaInteractome.reactome.highlightInputsByEntries(new Array(nodeName));
copaInteractome.displayInformation(nodeName, copaInteractome.copaProteinLink);
d3.selectAll('g.node#node-' + nodeName).classed('highlighted', true);
d3.selectAll('g.node.source').classed('highlighted', true);
d3.selectAll('path.source').classed('highlighted', true);

console.log('uni');
copaInteractome.circleView.displayPrimaryNames();
copaInteractome.reactome.displayPrimaryValues();
copaInteractome.go_processes.displayPrimaryValues();
copaInteractome.geneNames = false;
copaInteractome.displayInformation(copaInteractome.selectedProtein, copaInteractome.copaProteinLink);
copaInteractome.highlightUniprotAccessionsOption();

d3.selectAll('path.link.target-' + nodeName)
         .classed('target', true)
         .each(this._updateNodes('source', true));

d3.selectAll('path.link.source-' + nodeName)
         .classed('source', true)
         .each(this._updateNodes('target', true));
}} </script>", str);
        }
        else this.contentJS.Text = string.Format(@"<script> function startInteract(){{ }}</script>");

    }
}