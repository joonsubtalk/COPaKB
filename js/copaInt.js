function getURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
    );
}

copaInteractome.geneNames;
copaInteractome.copaProteinLink = "http://www.heartproteome.org/copa/ProteinInfo.aspx?QType=Protein%20ID&QValue="; //  http://www.heartproteome.org/copa/ProteinInfo.aspx?QType=Protein%20ID&QValue=P68134
copaInteractome.selectedProtein = "" + getURLParameter("iid");

/* Start COPa interactome */
copaInteractome.initCopaInteractome = function () {

    /* INTERACTIONS */
    copaInteractome.circleView = new Biojs.InteractionsCircleView({
        "target": "interactionsView",
        "inputData": copaInteractome.interactionsInputData,
        "width": 1030,
        "height": 1030,
        "tension": 0.9,
        "alternativeNames": copaInteractome.uniprotAccs2geneNames
    });

    /* GO PROCESSES */
    copaInteractome.go_processes = new Biojs.SelectionList({
        "target": "goProcesses",
        "name": "GO",
        "itemsPerColumn": 1,
        "alternativeValues": copaInteractome.uniprotAccs2geneNames,
        "inputData": copaInteractome.goProcessesInputData
    });

    /* REACTOME */
    copaInteractome.reactome = new Biojs.SelectionList({
        "target": "reactomePathways",
        "name": "Pathway",
        "itemsPerColumn": 2,
        "alternativeValues": copaInteractome.uniprotAccs2geneNames,
        "inputData": copaInteractome.reactomeInputData
    });


    copaInteractome.circleView.onNodeClick(
          function (event) {
              copaInteractome.selectedProtein = event.nodeName;
              if (event.highlighted) {
                  copaInteractome.go_processes.highlightEntries(new Array(event.nodeName), "highlighted");
                  copaInteractome.go_processes.unhighlightInputs();
                  copaInteractome.go_processes.highlightInputsByEntries(new Array(event.nodeName));
                  copaInteractome.reactome.highlightEntries(new Array(event.nodeName), "highlighted");
                  copaInteractome.reactome.unhighlightInputs();
                  copaInteractome.reactome.highlightInputsByEntries(new Array(event.nodeName));
              } else {
                  copaInteractome.go_processes.unhighlightEntries("highlighted");
                  copaInteractome.go_processes.unhighlightInputs();
                  copaInteractome.reactome.unhighlightEntries("highlighted");
                  copaInteractome.reactome.unhighlightInputs();
              }
              copaInteractome.displayInformation(event.nodeName, copaInteractome.copaProteinLink);
          }
    );

    copaInteractome.go_processes.onInputSelection(
          function (event) {
              copaInteractome.circleView.unselectAllNodes();
              copaInteractome.circleView.selectNodes(event.selectedEntries);
              //console.log(event);
              if (copaInteractome.reactome.getCheckedInputs().length > 0) {
                  copaInteractome.reactome.uncheckInputs();
              }
          }
    );

    copaInteractome.reactome.onInputSelection(
          function (event) {
              copaInteractome.circleView.unselectAllNodes();
              copaInteractome.circleView.selectNodes(event.selectedEntries);
              if (copaInteractome.go_processes.getCheckedInputs().length > 0) {
                  copaInteractome.go_processes.uncheckInputs();
              }
          }
    );

    /* Display Gene Names */
    if (copaInteractome.geneNames != false) {
        copaInteractome.displayGeneNames();
    } else {
        copaInteractome.highlightUniprotAccessionsOption();
    }

}


copaInteractome.highlightGeneNamesOption = function () {
    jQuery('.gene_names_opt').css("font-weight", "bold");
    jQuery('.uniprot_accessions_opt').css("font-weight", "normal");
}

copaInteractome.highlightUniprotAccessionsOption = function () {
    jQuery('.gene_names_opt').css("font-weight", "normal");
    jQuery('.uniprot_accessions_opt').css("font-weight", "bold");
}

/* Display gene names */
copaInteractome.displayGeneNames = function () {
    if (copaInteractome.geneNames != true) {
        console.log("gene");
        copaInteractome.circleView.displayAlternativeNames();
        copaInteractome.reactome.displayAlternativeValues();
        copaInteractome.go_processes.displayAlternativeValues();
        copaInteractome.geneNames = true;
        copaInteractome.displayInformation(copaInteractome.selectedProtein, copaInteractome.copaProteinLink);
        copaInteractome.highlightGeneNamesOption();
        startInteract();
    }
}

/* Display uniprot accessions */
copaInteractome.displayUniprotAccessions = function () {
    if (copaInteractome.geneNames != false) {
        console.log("uni");
        copaInteractome.circleView.displayPrimaryNames();
        copaInteractome.reactome.displayPrimaryValues();
        copaInteractome.go_processes.displayPrimaryValues();
        copaInteractome.geneNames = false;
        copaInteractome.displayInformation(copaInteractome.selectedProtein, copaInteractome.copaProteinLink);
        copaInteractome.highlightUniprotAccessionsOption();
    }
}

/* Display uniprot accessions */
copaInteractome.toggleSelectionListProteinDisplay = function () {
    jQuery('.slc_entry_legend').toggle();
}

/* Display uniprot accessions */
copaInteractome.clearAllHighlights = function () {
    jQuery('#proteinInformation').html('');
    copaInteractome.circleView.unselectAllNodes();
    copaInteractome.circleView.unhighlightNodes();
    copaInteractome.go_processes.uncheckInputs();
    copaInteractome.reactome.uncheckInputs();
    copaInteractome.go_processes.unhighlightEntries('highlighted');
    copaInteractome.go_processes.unhighlightInputs();
    copaInteractome.reactome.unhighlightEntries('highlighted');
    copaInteractome.reactome.unhighlightInputs();
}



/* Display interaction information */

copaInteractome.displayInformation = function (interactor, link) {
    var container = jQuery("#proteinInformation");
    container.html("");
    if (interactor != null) {
        var interactors = copaInteractome.circleView.getInteractions(interactor);
        //                    var interactorsString = "";
        var interactorsHtml = "";
        jQuery.each(interactors, function (key, value) {
            var valueText = value;
            if (copaInteractome.geneNames) {
                var geneName = copaInteractome.uniprotAccs2geneNames[value];
                if (geneName != undefined) {
                    if (geneName != "") {
                        valueText = geneName;
                    }
                }
            }
            interactorsHtml += '<a target="_copa" class="info_interactorProtein" href="' + link + value + '">' + valueText + '</a>';
        });
        if (interactorsHtml.length > 0) {
            var interactorText = interactor;
            if (copaInteractome.geneNames) {
                var geneName = copaInteractome.uniprotAccs2geneNames[interactor];
                if (geneName != undefined) {
                    if (geneName != "") {
                        interactorText = geneName;
                    }
                }
            }
            var html = '';
            html += '<h5>Selected protein</h5>';
            html += '<div class="info_selectedProtein"><a target="_copa" href="' + link + interactor + '">' + interactorText + '</a></div>';
            html += '<h5>Interactors</h5>';
            html += '<div class="info_interactors">' + interactorsHtml + '</div>';
            html += '<div style="position: relative;display: block; clear: both;"></div>';
            html = '<div class="info_panel">' + html + '</div>';
            jQuery(html).appendTo(container);
        }

    }
};