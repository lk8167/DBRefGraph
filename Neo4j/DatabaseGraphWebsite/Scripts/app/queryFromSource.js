var app = angular.module("DPGraph", []);

app.controller("queryFromSourceNode", function ($scope, $http) {
   
    $scope.targetNodes = [];
    $scope.sourceNodes = [];

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

    $scope.query = function () {
        var getTargetNodesUrl = 'api/DatabaseGraph/GetNodesFromSource?sourceNodeType=' + $scope.sourceNodeType + "&sourceNodeName=" + $scope.sourceNodeName;
        var config = {
            header: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        };
        $http.get(getTargetNodesUrl, config).then(
            function (response, status, headers, config) {
                $scope.targetNodes = response.data;
            }, function (data, status, headers, config) {

            }
        );
    }

    $scope.next = function (nodeType, nodeName) {
        $scope.sourceNodeType = nodeType;
        $scope.loadSourceNodeNamesByNodeType();
        $scope.sourceNodeName = nodeName;
        $scope.query();
    }
})