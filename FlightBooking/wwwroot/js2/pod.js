define(['underscore', 'backbone', 'moment'],
    function (_, Backbone) {
        var pod = {
            seeds: [],
            create: function (name, attrs) {
                ok = true;
                _.each(this.seeds, function (obj) {
                    if (obj.name == name) {
                        ok = false
                    };
                });
                if (ok)
                    var newSeed = {
                        name: name,
                        attrs: attrs
                    }
                this.seeds.push(newSeed);
            },
            update: function (name, newObj, changeLog) {
                if (changeLog) {
                    var oldObj = _.findWhere(this.seeds, { name: name }).attrs;
                    var oldObjArr = _.toArray(oldObj)
                    var newObjArr = _.toArray(newObj);
                    var diffValue = _.difference(newObjArr, oldObjArr)[0];
                    var attrName;
                    _.each(newObj, function (e, i) {
                        if (e === diffValue)
                            attrName = i;
                    });
                    if (attrName !== undefined) {
                        var oldVal = oldObj[attrName] instanceof Date ? moment(oldObj[attrName]).format('DD/MM/YYYY hh:mmA') : oldObj[attrName];
                        var newVal = newObj[attrName] instanceof Date ? moment(newObj[attrName]).format('DD/MM/YYYY hh:mmA') : newObj[attrName];
                        var data = {
                            life_id: pod.take('life-id'),
                            message: 'Field ' + attrName + ' was altered from value ' + oldVal + ', to new value ' + newVal
                        };
                        // this.sendChangeLog(data);
                    }
                };
                _.each(this.seeds, function (obj) {
                    if (obj.name == name) {
                        obj.attrs = newObj;
                    }
                });
            },
            take: function (name) {
                var seed = null;
                _.find(this.seeds, function (obj) {
                    if (obj.name == name)
                        seed = obj.attrs;
                });
                return seed;
            },
            createFromCollection: function (name, col) {
                obj = [];
                _.each(col.models, function (e) {
                    obj.push(e.attributes);
                });
                this.seeds.push({ name: name, attrs: obj });
            }
            //sendChangeLog: function (data) {
            //    $.ajax({
            //        url: '/event/changelog',
            //        type: 'POST',
            //        data: JSON.stringify(data),
            //        contentType: 'application/json; charset=UTF-8'
            //    });
            //}
        };
        this.pod = pod;
    });