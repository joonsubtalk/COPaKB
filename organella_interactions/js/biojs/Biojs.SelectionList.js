/** 
 * Component displaying an interactive key value checkbox list
 * 
 * @class
 * @extends Biojs
 * 
 * @author <a href="rafael@ebi.ac.uk">Rafael C Jimenez</a>
 * @version 1.0.0
 * @category 0
 * 
 * @requires <a href='http://code.jquery.com/jquery-1.9.1.min.js'>jQuery Core 1.9.1</a>
 * @dependency <script language="JavaScript" type="text/javascript" src="../biojs/dependencies/jquery/jquery-1.9.1.min.js"></script>
 *
 * @requires <a href='http://damirfoy.com/iCheck'>iCheck v0.9.1</a>
 * @dependency <script language="JavaScript" type="text/javascript" src="../biojs/dependencies/icheck/jquery.icheck.v0.9.1.js"></script>
 *
 * @requires <a href='../biojs/css/icheck/minimal/minimal.css'>iCheck minimal</a>
 * @dependency <link href="../biojs/css/icheck/minimal/minimal.css" rel="stylesheet" type="text/css" />
 * 
 * @param {Object} options An object with the options to display the component.
 *    
 * @option {string} target 
 *    Identifier of the DIV tag where the component should be displayed.
 * 
 * @option {Object} inputData
 * 	  Input data
 *     
 * @example 
 * var instance = new Biojs.SelectionList({
 * 		"target": "YourOwnDivId"
 * 		
 * });	
 * 
 */
Biojs.SelectionList = Biojs.extend (
/** @lends Biojs.SelectionList# */
{
  constructor: function (options) {
      Biojs.console.enable();


      this._container = jQuery("#"+this.opt.target);
      this._componentPanel = 0;
      this._componentPrefix = "slc";
      this._componentName = this._encodeWhiteSpace(this.opt.name);
      this._componentClassPrefix = this._componentPrefix + "-" + this._componentName;
      this._componentInputClassName = this._componentClassPrefix + "-input";
      this._checkedInputs = new Array(); // Collect checked inputs
      this._inverseInputData = this.getInverseInputData();
      this._drawPanel();
      this._applyIcheck(this._componentInputClassName);
      //Biojs.console.log(this._isNaturalNumber(0/3));

      var self = this;
      jQuery('.'+this._componentInputClassName)
          .on('ifChecked',function(event){
              self._highlight(this.name);
              self.raiseEvent(
              				"onInputSelection",
              				{ action: "checked",
                              input:this.name,
                              selectedInputs: self._checkedInputs,
                              selectedEntries: self._getEntries(self._checkedInputs)
                            }
              		);
          });
      jQuery('.'+this._componentInputClassName)
          .on('ifUnchecked',function(event){
              self._unhighlight(this.name);
              //console.log(event);
              self.raiseEvent(
              				"onInputSelection",
              				{ action: "unchecked",
                              input:this.name,
                              selectedInputs: self._checkedInputs,
                              selectedEntries: self._getEntries(self._checkedInputs)
                            }
              		);
          });



  },


    _hideEntryLegend: function(){
        jQuery('.'+self._componentPrefix+'-'+self._componentName+'-entry_legend').hide(200);
    },

    _showEntryLegend: function(){
        jQuery('.'+self._componentPrefix+'-'+self._componentName+'-entry_legend').show(200);
    },

    _getEntries: function(selectedInputs){
        var self = this;
        var entries = new Array();
        jQuery.each(selectedInputs, function(key, value) {
            var newEntries = self.opt.inputData[value];
            entries = jQuery.merge(entries, newEntries);
        });
        return jQuery.unique(entries);
    },

    _getInputs: function(selectedEntries){
        var self = this;
        var inputs = new Array();
        jQuery.each(selectedEntries, function(key, value) {
            var newInputs = self._inverseInputData[value];
            if(newInputs != undefined){
                inputs = jQuery.merge(inputs, newInputs);
            }
        });
        return jQuery.unique(inputs);
    },

    _removeItemFromArray: function(arr, itemtoRemove){
        var newArray = new Array();
        jQuery.each(arr, function(key, value) {
            if(value != itemtoRemove){
                newArray.push(value);
            }
        });
        return newArray;
    },

    //very expensive try not to use it, or just use it for one or few terms
    checkInputs: function(selectedInputs){
        var self = this;
        //console.log(selectedInputs);
        if(typeof selectedInputs != 'undefined'){
            jQuery.each(selectedInputs, function(key, value) {
                jQuery("input."+self._componentClassPrefix+"-input[name=\""+value+"\"]").iCheck('check');
            });
        } else {
            jQuery("input."+self._componentClassPrefix+"-input").iCheck('check');
        }
    },


        //todo: it does not work. "iCheck('check');" does not update
//        checkInputs: function(selectedInputs){
//            var self = this;
//            if(typeof selectedInputs != 'undefined'){
//                //Highlight
//                self.highlightInputs(selectedInputs,'selected');
//                self.highlightEntriesByInput(selectedInputs,'selected');
//                jQuery.each(selectedInputs, function(key, value) {
//                    jQuery("input."+self._componentClassPrefix+"-input[name=\""+value+"\"]").parent().addClass("checked");
//                    jQuery("input."+self._componentInputClassName).iCheck('update');
//                });
//            } else {
//                //Check all
//                this._checkedInputs = self.getKeysFromInputData();
//                //Unhighlight all
//                self.highlightInputs();
//                self.highlightEntries();
//                jQuery("div." + self._componentPrefix+'-'+self._componentName + " div.icheckbox_minimal").addClass("checked");
//                //jQuery("input."+self._componentInputClassName).iCheck('check');
//            }
//        },

    uncheckInputs: function(selectedInputs){
        var self = this;
        if(typeof selectedInputs != 'undefined'){
            //Unhighlight
            //self.unhighlightInputs(selectedInputs,'selected');
            self.highlightEntriesByInput(selectedInputs,'selected');
            jQuery.each(selectedInputs, function(key, value) {
                //Unckeck
                this._checkedInputs = this._removeItemFromArray(this._checkedInputs, value); // remove item from array
                jQuery("input."+self._componentClassPrefix+"-input[name=\""+value+"\"]").parent().removeClass("checked");
                jQuery("input."+self._componentInputClassName).iCheck('update');
            });
        } else {
            //Uncheck all
            this._checkedInputs = new Array();
            //Unhighlight  all
            //self.unhighlightInputs();
            self.unhighlightEntries();
            jQuery("div." + self._componentPrefix+'-'+self._componentName + " div.icheckbox_minimal").removeClass("checked");
            jQuery("input."+self._componentInputClassName).iCheck('update');
        }
    },


    _highlight: function(selectedInput){
        //this.highlightInputs(new Array(selectedInput),'selected');
        this._checkedInputs.push(selectedInput);
        this.highlightEntriesByInput(this._checkedInputs,'selected');
    },

    _unhighlight: function(selectedInput){
        //this.unhighlightInputs(new Array(selectedInput),'selected');
        this._checkedInputs = this._removeItemFromArray(this._checkedInputs, selectedInput); // remove item from array
        this.highlightEntriesByInput(this._checkedInputs,'selected');
    },




    highlightInputs: function(inputItems, className){
        var self = this;
        if(typeof className == 'undefined'){
            className = "selected";
        }
        if(typeof inputItems == 'undefined'){
            jQuery.each(self.opt.inputData, function(key, value) {
                jQuery("label."+self._componentClassPrefix+"-label[title=\""+key+"\"]").addClass(className);
            });
        } else {
            jQuery.each(inputItems, function(key, value) {
                jQuery("label."+self._componentClassPrefix+"-label[title=\""+value+"\"]").addClass(className);
            });
        }
   	},

    unhighlightInputs: function(selectedInput, className){
        var self = this;
        if(typeof className == 'undefined'){
            className = 'selected highlighted';
        }
        if(typeof selectedInput == 'undefined'){
            jQuery.each(self.opt.inputData, function(key, value) {
                jQuery("label."+self._componentClassPrefix+"-label[title=\""+key+"\"]").removeClass(className);
            });
        } else {
            jQuery.each(selectedInput, function(key, value) {
                jQuery("label."+self._componentClassPrefix+"-label[title=\""+value+"\"]").removeClass(className);
            });
        }
   	},


    highlightEntriesByInput: function(selectedInputs, className){
        this.highlightEntries(this._getEntries(selectedInputs), className);
   	},


    highlightInputsByEntries: function(selectedEntries, className){
        var selectedInputs = this._getInputs(selectedEntries);
        this.highlightInputs(selectedInputs, className);
    },




    highlightEntries: function(entries, className){
        var self = this;
        if(typeof className == 'undefined'){
            className = "selected";
        }

        if(typeof entries == 'undefined'){ // highlight all
            jQuery("span."+self._componentClassPrefix+"-entry").addClass(className);
        } else { // selective highlight
            this.unhighlightEntries(className);
            jQuery.each(entries, function(key, value) {
                jQuery("span."+self._componentClassPrefix+"-entry[title=\""+value+"\"]").addClass(className);
            });
        }
   	},
    unhighlightEntries: function(className){
        var self = this;
        if(typeof className == 'undefined'){
            className = 'selected highlighted';
        }
        jQuery("span."+self._componentClassPrefix+"-entry").removeClass(className);
   	},

    /*
     * Function: Biojs.EnsemblGeneSummaryView._drawTemplate
     * Purpose:  Clear data inside the main container
     * Returns:  -
     * Inputs:  -
     */
	_clearData: function(){
		this._container.html("");
	},

    _encodeWhiteSpace: function(string){
        return string.replace(/ /g, '_');
    },

    _decodeWhiteSpace: function(string){
        return string.replace(/_/g, ' ');
    },


    _descSortInputData: function(){
        var sortable = [];
        jQuery.each(this.opt.inputData, function(key, value) {
            sortable.push([key, value.length]);
        });
        sortable.sort(function(a, b) {return a[1] - b[1]});
        return sortable;
    },

    _drawPanel: function(){
        var self = this;
        var rowNum = 0;
        var panel = "p" + this._componentPanel; this._componentPanel++;
        var inputDataSorting = this._descSortInputData();


        var div_panel = jQuery('<div class="'+self._componentPrefix+'-'+self._componentName+' '+self._componentPrefix+'_panel" id="'+self._componentPrefix+'-'+self._componentName+'-'+panel+'"></div>').appendTo(this._container);
        var itemCount = 0;
        var ul;
//        jQuery.each(this.opt.inputData, function(key, value) {
        jQuery.each(inputDataSorting, function(key, value) {
            var inputName = value[0];
            var inputValues = self.opt.inputData[inputName];
            /* Add UL */
            //if(self._isNaturalNumber(itemCount/self.opt.itemsPerColumn) == true){
                var row = "i" + rowNum;
                ul = jQuery('<div class="'+self._componentPrefix+'_list '+self._componentPrefix+'_'+row+'"></div>').appendTo(div_panel);
                rowNum++;
            //}
            itemCount++
            /* Add list elements */
            //var li = jQuery('<li></li>');
            ul.append('<input type="checkbox" name="'+inputName+'" class="'+self._componentInputClassName+' '+self._componentPrefix+'_entry_input" determinate="false" id="'+self._componentPrefix+'-'+self._componentName+'-'+panel+'-'+self._encodeWhiteSpace(inputName)+'-input">');
            ul.append('<label title="'+inputName+'" class="'+self._componentPrefix+'-'+self._componentName+'-label '+self._componentPrefix+'_entry_label" id="'+self._componentPrefix+'-'+self._componentName+'-'+panel+'-'+self._encodeWhiteSpace(inputName)+'-label" for="'+self._componentPrefix+'-'+self._componentName+'-'+panel+'-'+self._encodeWhiteSpace(inputName)+'-input">'+inputName+'</label>');
            var entry_legend = jQuery('<div class="'+self._componentPrefix+'-'+self._componentName+'-entry_legend '+self._componentPrefix+'_entry_legend" id="'+self._componentPrefix+'-'+self._componentName+'-'+panel+'-'+self._encodeWhiteSpace(inputName)+'-legend"></div>').appendTo(ul);
            if(self.opt.entryLegend == false){
                entry_legend.hide();
            }
            jQuery.each(inputValues, function(key2, inputValue) {
                entry_legend.append('<span title="'+inputValue+'" class="'+self._componentPrefix+'_entry '+self._componentPrefix+'-'+self._componentName+'-entry '+self._componentPrefix+'-'+self._componentName+'-'+panel+'-'+self._encodeWhiteSpace(inputName)+'-entry '+self._encodeWhiteSpace(inputValue)+'">'+inputValue+'</span>');
            });
            //li.appendTo(ul);
        });
        jQuery('<div style="position: relative;display: block; clear: both;"></div>').appendTo(this._container);

   	},

    _applyIcheck: function(className){
        this._icheck = jQuery('.'+className).iCheck({
            checkboxClass: 'icheckbox_minimal',
            radioClass: 'iradio_minimal',
            increaseArea: '20%' // optional
        });
    },


    _isNaturalNumber: function(n){
       n = n.toString(); // force the value incase it is not
       var n1 = Math.abs(n),
           n2 = parseInt(n, 10);
       return !isNaN(n1) && n2 === n1 && n1.toString() === n;
   },



	getInverseInputData: function(){
        var inverseInputData = new Object();
        jQuery.each(this.opt.inputData, function(key, value) {
            //Biojs.console.log(key);
            jQuery.each(value, function(key2, value2) {
                //Biojs.console.log(value2);
                if(typeof inverseInputData[value2] == 'undefined'){
                    inverseInputData[value2] = new Array(key);
                } else {
                    inverseInputData[value2].push(key);
                }
            });
        });
        return inverseInputData;
	},

    getInputData: function(){
        return this.opt.inputData;
   	},

    getKeysFromInputData: function(){
        var inputs = new Array();
        jQuery.each(this.opt.inputData, function(key, value) {
            inputs.push(key);
        });
        return inputs;
   	},

    getValuesFromInputData: function(){
        var values = new Array();
        jQuery.each(this.opt.inputData, function(key, value) {
            values.push(value);
        });
        return values;
    },

    getCheckedInputs: function(){
        return this._checkedInputs;
   	},

    displayAlternativeValues: function(){
        var self = this;
        jQuery.each(this.opt.alternativeValues, function(key, value) {
            if(value != ""){
                jQuery("div."+self._componentClassPrefix+" span[title=\""+key+"\"]").text(value);
            }
        });
    },

        displayPrimaryValues: function(){
        var self = this;
        jQuery.each(this.opt.alternativeValues, function(key, value) {
            jQuery("div."+self._componentClassPrefix+" span[title=\""+key+"\"]").text(key);
        });
    },

    displayAlternativeKeys: function(){
        var self = this;
        jQuery.each(this.opt.alternativeKeys, function(key, value) {
            if(value != ""){
                jQuery("div."+self._componentClassPrefix+" label[title=\""+key+"\"]").text(value);
            }
        });
    },

    displayPrimaryKeys: function(){
        var self = this;
        jQuery.each(this.opt.alternativeKeys, function(key, value) {
            jQuery("div."+self._componentClassPrefix+" label[title=\""+key+"\"]").text(key);
        });
    },

  /**
   *  Default values for the options
   *  @name Biojs.SelectionList-opt
   */
  opt: {
     "target": "YourOwnDivId",
     "inputData": {
            "Pathway 1":["Value A","Value C","Value D"],
            "Pathway 2":["Value A","Value B"],
            "Pathway 3":["Value B","Value E"],
            "Pathway 4":["Value E"],
            "Pathway 5":["Value B","Value F"],
            "Pathway 6":["Value A","Value F"],
            "Pathway 7":["Value A"]
        },
      "alternativeValues": {},
      "alternativeKeys": {},
      "name": "Pathway data",  //todo:make name mandatory
      "itemsPerColumn": 3,
      "entryLegend": true
  },
  
  /**
   * Array containing the supported event names
   * @name Biojs.SelectionList-eventTypes
   */
  eventTypes : [
      		/**
      		 * @name Biojs.SelectionList#onInputSelection
      		 * @event
      		 * @param {function} actionPerformed A function which receives a {@link Biojs.Event} object as argument.
	 * @eventData {string} action Action perform on input.
	 * @eventData {string} input Input item selected
	 * @eventData {Array} selectedInputs Selected input items
	 * @eventData {Array} selectedEntries Selected entries (input items values)
	 *
      		 * @example
      		 * instance.onInputSelection(
      		 *    function( event ) {
      		 *       alert( event.input + " " + event.action );
      		 *    }
      		 * );
      		 *
      		 **/
      		"onInputSelection"
  ]
  
});


