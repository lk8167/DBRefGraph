var app = angular.module("DatabaseGraph", []);

app.controller("AddNewNode", function ($scope, $http) {
    $scope.showDatabase = false;
    $scope.showWebPage = false;

    $scope.displayOnNodeType = function () {
        if ($scope.NodeType == '') {
            $scope.showDatabase = false;
            $scope.showWebPage = false;
        }
        else if ($scope.NodeType == 'Page') {
            $scope.showDatabase = false;
            $scope.showWebPage = true;
        }
        else {
            $scope.showDatabase = true;
            $scope.showWebPage = false;
        }
    }

    $scope.ResetControls = function () {
        $scope.showDatabase = false;
        $scope.showWebPage = false;
        $scope.NodeType = '';
        $scope.dbSchema = '';
        $scope.dbName = '';
        $scope.pageModule = '';
        $scope.pageName = '';

    }

    $scope.AddNewNode = function () {
        if ($scope.NodeType == '') {
            alert("Please select node type");
            return;
        }
        else if ($scope.NodeType == 'Page') {
            if ($scope.pageModule == '') {
                alert("Please select page module");
                return;
            }
            if ($scope.pageName == '') {
                alert("Please input page name");
                return;
            }
        }
        else {
            if ($scope.dbSchema == '') {
                alert("Please input database schema");
                return;
            }
            if ($scope.dbName == '') {
                alert("Please input database object name");
                return;
            }
        }
        var newNodeUrl = "api/DatabaseGraph/CreateNode";
        var config = {
            header: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        };
        var newNode = {
            NodeType: $scope.NodeType,
            DBSchema: $scope.dbSchema,
            DBName: $scope.dbName,
            PageModule: $scope.pageModule,
            PageName: $scope.pageName
        };
        $http.post(newNodeUrl, newNode, config).then(
            function (data, status, headers, config) {
                alert("Save new node succeed");
                $scope.ResetControls();
            }, function (data, status, headers, config) {
                alert("Save new node failed");
            }
        );
    }
});