angular.module("umbraco").controller("communityDashboard", function ($scope, contentResource, entityResource) {

    // Find the community section
    entityResource.getByQuery("//community", -1, "Document").then(function (document) {

        // Get the community members
        contentResource.getChildren(document.id).then(function (response) {
            var community = response.items;

            // For each community member
            _.each(community, function (member) {

                // Get the last updated date
                var date = moment(member.updateDate);

                // Set true if more than 6 days since last update
                member.outdated = date.diff(Date.now(), "days") <= -6;

                // Time from last update
                member.diff = date.fromNow();

                // Get the GitHub Username value
                member.avatar = _.findWhere(member.properties, { 'alias': 'githubProfile' }).value;
            })

            // Add to scope so the it's available in the view
            $scope.community = community;

        });

    });

});