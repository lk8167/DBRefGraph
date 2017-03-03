var app = angular.module("DPGraph", []);

app.controller("NewRelationship", function ($scope, $http) {
    $scope.sourceNodes = [];
    $scope.targetNodes = [];

    $scope.loadSourceNodeNamesByNodeType = function () {
        if ($scope.sourceNodeType == '') {
            $scope.sourceNodes = [];
            return;
        }
        var getNodeListUrl = 'api/DatabaseGraph/GetNodesByType?nodeType=' + $scope.sourceNodeType;
        var config = {
            header: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        };
        $http.get(getNodeListUrl, config).then(
            function (response, status, headers, config) {
                $scope.sourceNodes = response.data;
            }, function (data, status, headers, config) {
                
            }
        );
    }

    $scope.loadTargetNodeNamesByNodeType = function () {
        if ($scope.targetNodeType) {
            $scope.targetNodes = [];
        }
        var getNodeListUrl = 'api/DatabaseGraph/GetNodesByType?nodeType=' + $scope.targetNodeType;
        var config = {
            header: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        };
        $http.get(getNodeListUrl, config).then(
            function (response, status, headers, config) {
                $scope.targetNodes = response.data;
            }, function (data, status, headers, config) {

            }
        );
    }

    $scope.ResetControls = function () {
        $scope.sourceNodeType = "";
        $scope.targetNodeType = "";
        $scope.sourceNodes = [];
        $scope.targetNodes = [];
    }

    $scope.addNewRelationship = function () {
        var newRelationshipUrl = 'api/DatabaseGraph/CreateRelationship';
        var config = {
            header: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        };
        var newRelationship = {
            SourceNodeType: $scope.sourceNodeType,
            SourceNodeName: $scope.sourceNodeName,
            TargetNodeType: $scope.targetNodeType,
            TargetNodeName: $scope.targetNodeName
        };
        $http.post(newRelationshipUrl, newRelationship, config).then(
            function (response, status, headers, config) {
                alert("Save new relationship succeed");
                $scope.ResetControls();
            }, function (response, status, headers, config) {
                if (response.data) {
                    alert(response.data.ExceptionMessage);
                }
                else {
                    alert("Save new relationship failed");
                }
            }
        );
}
});