var app = angular.module("DPGraph", []);

app.controller("queryFromTargetNode", function ($scope, $http) {

    $scope.targetNodes = [];
    $scope.sourceNodes = [];

    $scope.loadTargetNodeNamesByNodeType = function () {
        if ($scope.targetNodeType == '') {
            $scope.targetNodes = [];
            return;
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

    $scope.query = function () {
        var getSourceNodesUrl = 'api/DatabaseGraph/GetNodesFromTarget?targetNodeType=' + $scope.targetNodeType + "&targetNodeName=" + $scope.targetNodeName;
        var config = {
            header: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        };
        $http.get(getSourceNodesUrl, config).then(
            function (response, status, headers, config) {
                $scope.sourceNodes = response.data;
            }, function (data, status, headers, config) {

            }
        );
    }

    $scope.next = function (nodeType, nodeName) {
        $scope.targetNodeType = nodeType;
        $scope.loadTargetNodeNamesByNodeType();
        $scope.targetNodeName = nodeName;
        $scope.query();
    }
})