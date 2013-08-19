/** 
 * Component displaying interactions in a circle view
 * 
 * @class
 * @extends Biojs
 * 
 * @author <a href="mailto:rafael@ebi.ac.uk">Rafael C Jimenez</a>
 * @version 1.0.0
 * @category 0
 * 
 * @requires <a href='http://d3js.org/'>D3.js v3</a>
 * @dependency <script language="JavaScript" type="text/javascript" src="../biojs/dependencies/d3/d3.v2.5.js"></script>
 *
 *
 * @requires <a href='http://d3js.org/'>d3.layout</a>
 * @dependency <script language="JavaScript" type="text/javascript" src="../biojs/dependencies/d3.layout.js"></script>
 *
 *
 * @requires <a href='http://d3js.org/'>packages</a>
 * @dependency <script language="JavaScript" type="text/javascript" src="../biojs/dependencies/packages.js"></script>
 *
 * @requires <a href='../biojs/css/Biojs.InteractionsCircleView.css'>Biojs.InteractionsCircleView.css</a>
 * @dependency <link href="../biojs/css/Biojs.InteractionsCircleView.css" rel="stylesheet" type="text/css" />
 * 
 * @param {Object} options An object with the options to display the component.
 *    
 * @option {string} target 
 *    Identifier of the DIV tag where the component should be displayed.
 * 
 * @option {Array} inputData
 * 	  Input data
 *
 * 
 */

//todo: bring dependencies to BioJS

Biojs.InteractionsCircleView = Biojs.extend (
/** @lends Biojs.InteractionsCircleView# */
{
  constructor: function (options) {
    this._startD3();
    //this._playground();
  },

    _playground: function(){
        var div = d3.select('#'+this.opt.target).append("div");
        var span = d3.select('#'+this.opt.target).insert("span");
        var h1 = div.append("h1");
        h1.style("top", "60px");
        h1.attr("class", "hell")
        span.style("top", "70px");
        div.style("top", "80px");
    },

    _startD3: function(){
        Biojs.console.enable();
        var self = this;
        this._svg;


        var w = this.opt.width, //OPT width
            h = this.opt.height, //OPT height
            rx = w / 2,
            ry = h / 2,
            m0,
            rotate = 0;

        var splines = [];


        /* The cluster LAYOUT produces dendrograms */
        var cluster = d3.layout.cluster()
            .size([360, ry - 120])
            .sort(function(a, b) { return d3.ascending(a.key, b.key); });
        /* LAYOUT implementing Danny Holten's hierarchical edge bundling algorithm */
        var bundle = d3.layout.bundle();


        /* Constructs a new radial line generator */
        var line = d3.svg.line.radial()
            .interpolate("bundle")
            .tension(this.opt.tension)
            .radius(function(d) { return d.y; })
            .angle(function(d) { return d.x / 180 * Math.PI; });


        /* Create SVG in a new div container */
        // Chrome 15 bug: <http://code.google.com/p/chromium/issues/detail?id=98951>

        var container = d3.select('#'+this.opt.target)
            .append("div")
            .attr("class", Biojs.InteractionsCircleView.COMPONENT_PREFIX + "container")
            .style("width", w + "px")
            .style("height", h + "px")

        var div = container.insert("div", "h2")
            //.style("top", "-80px")
            //.style("left", "-160px")
            //.style("left", "-360px")
            .style("width", w + "px")
            .style("height", w + "px")
            .style("position", "absolute")
            .style("-webkit-backface-visibility", "hidden")
            .attr("class", Biojs.InteractionsCircleView.COMPONENT_PREFIX + "subcontainer");

        this._svg = div.append("svg:svg")
            .attr("width", w)
            .attr("height", w)
            .append("svg:g")
            .attr("transform", "translate(" + rx + "," + ry + ")");

        this._svg.append("svg:path")
            .attr("class", "arc")
            .attr("d", d3.svg.arc().outerRadius(ry - 120).innerRadius(0).startAngle(0).endAngle(2 * Math.PI))
            .on("mousedown", mousedown);


        //d3.json("flare-interactions2.json", function(classes) { //OPT json input
          var classes = this.opt.inputData;
          /* Apply layouts */
          var nodes = cluster.nodes(packages.root(classes)), // Apply cluster layout
              links = packages.interactions(nodes),
              splines = bundle(links); // Apply bundle layout
          /* Create svg panel for edges */
          var path = this._svg.selectAll("path.link")
              .data(links)
              .enter().append("svg:path")
              .attr("class", function(d) { return "link source-" + d.source.key + " target-" + d.target.key; })
              .attr("d", function(d, i) { return line(splines[i]); });
          /* Create svg panel for nodes */
          this._svg.selectAll("g.node")
              .data(nodes.filter(function(n) { return !n.children; }))
              .enter().append("svg:g")
              .attr("class", "node")
              .attr("id", function(d) { return "node-" + d.key; })
              .attr("transform", function(d) { return "rotate(" + (d.x - 90) + ")translate(" + d.y + ")"; })
              .append("svg:text")
              .attr("dx", function(d) { return d.x < 180 ? 8 : -8; })
              .attr("dy", ".31em")
              .attr("text-anchor", function(d) { return d.x < 180 ? "start" : "end"; })
              .attr("transform", function(d) { return d.x < 180 ? null : "rotate(180)"; })
              .text(function(d) { return d.key; })
              .on("mouseover", mouseover)
              .on("click", mouseclick)
              .on("mouseout", mouseout);
          /* Tension listener */
          d3.select("input[type=range]").on("change", function() {
            line.tension(this.value / 100);
            path.attr("d", function(d, i) { return line(splines[i]); });
          });
        //});



        /* Circle rotation */
        d3.select(window)
            .on("mousemove", mousemove)
            .on("mouseup", mouseup);

        function mouse(e) {
          return [(e.layerX) - rx, e.layerY - ry];
        }

        function mousedown() {
          m0 = mouse(d3.event);
          d3.event.preventDefault();
        }

        function mousemove() {
          if (m0) {
            var m1 = mouse(d3.event),
                dm = Math.atan2(cross(m0, m1), dot(m0, m1)) * 180 / Math.PI;
            div.style("-webkit-transform", "translateY(" + (ry - rx) + "px)rotateZ(" + dm + "deg)translateY(" + (rx - ry) + "px)");
          }
        }

        function mouseup() {
          if (m0) {
            var m1 = mouse(d3.event),
                dm = Math.atan2(cross(m0, m1), dot(m0, m1)) * 180 / Math.PI;

            rotate += dm;
            if (rotate > 360) rotate -= 360;
            else if (rotate < 0) rotate += 360;
            m0 = null;

            div.style("-webkit-transform", null);

            self._svg.attr("transform", "translate(" + rx + "," + ry + ")rotate(" + rotate + ")")
                     .selectAll("g.node text")
                     .attr("dx", function(d) { return (d.x + rotate) % 360 < 180 ? 8 : -8; })
                     .attr("text-anchor", function(d) { return (d.x + rotate) % 360 < 180 ? "start" : "end"; })
                     .attr("transform", function(d) { return (d.x + rotate) % 360 < 180 ? null : "rotate(180)"; });
          }
        }

        function cross(a, b) {
          return a[0] * b[1] - a[1] * b[0];
        }

        function dot(a, b) {
          return a[0] * b[0] + a[1] * b[1];
        }



        /* Node mouseover -> node + edges highlight*/
        function mouseclick(d) {
          self._mouseclick(d);
        }
        function mouseover(d) {
          self._mouseover(d);
  //          Biojs.console.log(d.key);
  //
  //          self._svg.selectAll("path.link.target-" + d.key)
  //                   .classed("target", true)
  //                   .each(updateNodes("source", true));
  //
  //          self._svg.selectAll("path.link.source-" + d.key)
  //                   .classed("source", true)
  //                   .each(updateNodes("target", true));
        }

        function mouseout(d) {
            self._mouseout(d);
  //          self._svg.selectAll("path.link.source-" + d.key)
  //                   .classed("source", false)
  //                   .each(updateNodes("target", false));
  //
  //          self._svg.selectAll("path.link.target-" + d.key)
  //                   .classed("target", false)
  //                   .each(updateNodes("source", false));
        }

  //      function updateNodes(name, value) {
  //        return function(d) {
  //          if (value) this.parentNode.appendChild(this);
  //            self._svg.select("#node-" + d[name].key).classed(name, value);
  //        };
  //      }
    },



    /*
     * Function: Biojs.InteractionsCircleView._updateNodes
     * Purpose:  -
     * Returns:  -
     * Inputs:  -
     */
	_updateNodes: function(name, value){
        var self = this;
        return function(d) {
          if (value) this.parentNode.appendChild(this);
            self._svg.select("#node-" + d[name].key).classed(name, value);
        };
	},



    /*
     * Function: Biojs.InteractionsCircleView._mouseclick
     * Purpose:  -
     * Returns:  -
     * Inputs:  -
     */
	_mouseclick: function(d){
        var highlighted = this.toggleHighlightNode(d.key);
        this.raiseEvent(
        				"onNodeClick",
        				{   "nodeName": d.key,
                            "highlighted": highlighted
                        }
        		);

	},

    /*
     * Function: Biojs.InteractionsCircleView._mouseout
     * Purpose:  -
     * Returns:  -
     * Inputs:  -
     */
	_mouseout: function(d){
        this._svg.selectAll("path.link.source-" + d.key)
                 .classed("source", false)
                 .each(this._updateNodes("target", false));

        this._svg.selectAll("path.link.target-" + d.key)
                 .classed("target", false)
                 .each(this._updateNodes("source", false));
	},

	/*
     * Function: Biojs.InteractionsCircleView._mouseover
     * Purpose:  -
     * Returns:  -
     * Inputs:  -
     */
	_mouseover: function(d){
        this._svg.selectAll("path.link.target-" + d.key)
                 .classed("target", true)
                 .each(this._updateNodes("source", true));

        this._svg.selectAll("path.link.source-" + d.key)
                 .classed("source", true)
                 .each(this._updateNodes("target", true));
	},


    /**
   	 * Select node and highlight (color) interactions.
   	 * @param {string} identifier Interactor identifier.
   	 *
   	 * @example
   	 * instance.selectNode("P07200");
   	 *
   	 */
    toggleHighlightNode: function(id){
        var newHighlight = true;
        if(this._svg.selectAll("g.node#node-" + id).classed("highlighted") == true){
            newHighlight = false;
        }
        this.unhighlightNodes();
        if(newHighlight){
            this.highlightNode(id);
        } else {
            this.unhighlightNode(id);
        }
        return newHighlight;
    },

    highlightNode: function (id) {
        this._svg.select("g.node#node-" + id).classed("highlighted", true);
        this._svg.selectAll("path.source-" + id).classed("highlighted", true);
        this._svg.selectAll('path.link.source-' + id).classed("highlighted", true);
        this._svg.selectAll('path.link.target-' + id)
                 .each(function (d, i) {
                     if (d.target.interactions != undefined) {
                         d.target.interactions.forEach(
                             function (node, index) {
                                 var nodeName = node;
                                 if (node.lastIndexOf(".") != -1) {
                                     nodeName = node.substring(node.lastIndexOf(".") + 1);
                                 }
                                 console.log(nodeName);
                                 d3.select("g.node#node-" + nodeName).classed("highlighted", true);
                             }
                         );
                     }
                 });
    },

    /* old code
    highlightNode: function(id){
        this._svg.selectAll("g.node#node-" + id).classed("highlighted", true);
        this._svg.selectAll("g.node.source").classed("highlighted", true);
        this._svg.selectAll("path.source").classed("highlighted", true);
   	},*/

    unhighlightNode: function(id){
        this._svg.selectAll("g.node#node-" + id).classed("highlighted", false);
   	},

    unhighlightNodes: function(){
        this._svg.selectAll("g.node.highlighted").classed("highlighted", false);
        this._svg.selectAll("path.highlighted").classed("highlighted", false);
   	},

    /**
   	 * Select node and highlight (color) interactions.
   	 * @param {string} identifier Interactor identifier.
   	 *
   	 * @example
   	 * instance.selectNode("P07200");
   	 *
   	 */
   	selectNode: function(id){
        //console.log("select " + id);  //RISE EVENT
        this._svg.selectAll("path.link.source-" + id)
                 .classed("selected", true);
        this._svg.selectAll("g.node#node-" + id)
                         .classed("selected", true);
                 //.each(this._updateNodes("target", false));
   	},
    /**
   	 * Unselect node and remove highlight (color).
   	 * @param {string} identifier Interactor identifier.
   	 *
   	 * @example
   	 * instance.unselectNode("P07200");
   	 *
   	 */
   	unselectNode: function(id){
        console.log("UNselect " + id);  //RISE EVENT
        this._svg.selectAll("path.link.source-" + id)
                 .classed("selected", false);
        this._svg.selectAll("g.node#node-" + id)
                                 .classed("selected", false);
                 //.each(this._updateNodes("target", false));
   	},
    /**
   	 * Select nodes and highlight (color) interactions.
   	 * @param {Array} identifiers List of interactor identifiers.
   	 *
   	 * @example
   	 * instance.selectNodes(["P07200","Q15052"]);
   	 *
   	 */
   	selectNodes: function(ids){
        for (var i = 0; i < ids.length; i++) {
            this.selectNode(ids[i]);
        }
   	},

    /**
   	 * Unselect nodes and remove highlight (color).
   	 * @param {Array} identifiers List of Interactor identifiers.
   	 *
   	 * @example
   	 * instance.unselectNodes(["P07200","Q15052"]);
   	 *
   	 */
   	unselectNodes: function(ids){
        for (var i = 0; i < ids.length; i++) {
            this.unselectNode(ids[i]);
        }
   	},
    /**
   	 * Unselect all nodes and remove highlight (color).
   	 *
   	 * @example
   	 * instance.unselectAllNodes();
   	 *
   	 */
   	unselectAllNodes: function(){
        //RISE EVENT
        this._svg.selectAll("path.link")
                 .classed("selected", false);
        this._svg.selectAll("g.node.selected")
                         .classed("selected", false);
                 //.each(this._updateNodes("target", false));
   	},

    getInteractions: function(interactor){
        var interactions = new Array();
        this.opt.inputData.forEach(function(d,i) {
            if(d.key == interactor){
                d.interactions.forEach(function(s,j){
                    var dotLoc = s.lastIndexOf(".");
                    if(dotLoc != -1 && dotLoc+1 < s.length){
                        var acc = s.substring(dotLoc+1, s.length);
                        interactions.push(acc);
                    }
                })

            }
        });
        return interactions;
    },

    displayAlternativeNames: function(){
        var self = this;
        for (var key in this.opt.alternativeNames) {
           if (this.opt.alternativeNames.hasOwnProperty(key) && self.opt.alternativeNames[key] != "") {
               d3.select(d3.select("g#node-"+key+" text").node()).text(self.opt.alternativeNames[key]);
           }
        }
    },

    displayPrimaryNames: function(){
        var self = this;
        for (var key in this.opt.alternativeNames) {
           if (this.opt.alternativeNames.hasOwnProperty(key)) {
               d3.select(d3.select("g#node-"+key+" text").node()).text(key);
           }
        }
    },


  /**
   *  Default values for the options
   *  @name Biojs.InteractionsCircleView-opt
   */
  opt: {
     target: "YourOwnDivId",
     inputData: [],
     tension: 0.85,
     width:1280,
     height: 800,
     alternativeNames: {}

  },
  
  /**
   * Array containing the supported event names
   * @name Biojs.InteractionsCircleView-eventTypes
   */
  eventTypes : [
       /**
        * @name Biojs.SelectionList#onNodeClick
        * @event
        * @param {function} actionPerformed A function which receives a {@link Biojs.Event} object as argument.
        * @eventData {string} nodeName Node name.
        *
        * @example
        * instance.onNodeClick(
        *    function( event ) {
        *       alert( event.nodeName );
        *    }
        * );
        *
        **/
		"onNodeClick"
  ]

},{
	// Some static values
	COMPONENT_PREFIX: "icv_"
});



