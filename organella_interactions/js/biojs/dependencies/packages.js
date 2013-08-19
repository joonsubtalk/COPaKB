(function() {
  packages = {

    // Lazily construct the package hierarchy from class interactors.
    root: function(classes) {
      var map = {};

      function find(interactor, data) {
        var node = map[interactor], i;
        if (!node) {
          node = map[interactor] = data || {interactor: interactor, children: []};
          if (interactor.length) {
            node.parent = find(interactor.substring(0, i = interactor.lastIndexOf(".")));
            node.parent.children.push(node);
            node.key = interactor.substring(i + 1);
          }
        }
        return node;
      }

      classes.forEach(function(d) {
        find(d.interactor, d);
      });

      return map[""];
    },

    // Return a list of interactions for the given array of nodes.
    interactions: function(nodes) {
      var map = {},
          interactions = [];

      // Compute a map from interactor to node.
      nodes.forEach(function(d) {
        map[d.interactor] = d;
      });

      // For each import, construct a link from the source to target node.
      nodes.forEach(function(d) {
        if (d.interactions) d.interactions.forEach(function(i) {
          interactions.push({source: map[d.interactor], target: map[i]});
        });
      });

      return interactions;
    }
  };
})();