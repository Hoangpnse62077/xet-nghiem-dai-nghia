/*
 *  Class: AppointmentSuggestor
 *  Author: DucBM
 *  Usage: AppointmentSuggestor.CalcTheBestTour(sampleDtos)
 *  Return: [{SampleId, SlotId}]
 */
var AppointmentSuggestor = {
    CalcTheBestTour: function (selectedSampleDtos) {

        const MAX = 24 * 60 * 60; // total wait time is never exceed 24 hours

        var sampleIds = [];
        var samplesWithSlots = [];
        var sampleIdOrder = []; // vd: Mau -> Te Bao -> Dich -- [1,3,5]
        var sampleOrderWithSlots = [];

        var dA = []; // dA[i] = true/false; for make Permutations
        var n = selectedSampleDtos.length;
        var minWait = MAX; // MAX

        var result = [];

        var prepareDataStructure = function () {
            // init sampleIds
            for (var i = 0; i < selectedSampleDtos.length; i++) {
                sampleIds.push(selectedSampleDtos[i].SampleId);
            }

            // init samplesWithSlots
            var arr = [];
            for (var i = 0; i < selectedSampleDtos.length; i++) {
                var sampleDto = selectedSampleDtos[i];
                arr.push([]);
                for (var j = 0; j < sampleDto.SlotOptions.length; j++) {
                    // object SlotOption: {SlotId: 1, StartTime: 14400, FinishTime: 15000, IsAvailable: true}
                    var slot = sampleDto.SlotOptions[j];
                    // filter by StartTime > comingTime and Is Available
                    if (slot.IsAvailable == true) {
                        var item = {
                            StartTime: slot.StartTime,
                            FinishTime: slot.FinishTime,
                            SlotId: slot.SlotId
                        };
                        arr[i].push(item);
                    } // end if
                } // end for j
            } // end for i
            samplesWithSlots = arr;

            // init ...
            for (var i = 0; i < n; i++) {
                dA[i] = false;
                sampleIdOrder.push([]);
                sampleOrderWithSlots.push([]);
            }
        }

        // generate a new permutation of samples
        var permute = function (i) {
            if (i == n) {
                dynamicProgram();
                return;
            }
            for (var j = 0; j < n; j++) {
                if (dA[j] == false) {
                    sampleIdOrder[i] = sampleIds[j];
                    sampleOrderWithSlots[i] = samplesWithSlots[j];
                    dA[j] = true;
                    permute(i + 1);
                    dA[j] = false;
                }
            }
        }

        var dynamicProgram = function () {
            var dp = []; // dp: total time for waiting (waiting between slots)
            var trace = [];
            var A = sampleOrderWithSlots;
            // dp
            for (var i = 0; i < n; i++) {
                dp.push([]); // bang 2 chieu. them 1 dong de tinh gia tri du lieu
                trace.push([]);
                for (var j = 0; j < A[i].length; j++) {
                    var dpItem = MAX;
                    var traceItem = -1;
                    if (i == 0) {
                        //dpItem = A[i][j].StartTime - comingTime;
                        dpItem = 0;
                    } else {
                        dpItem = MAX;
                        for (var k = 0; k < dp[i - 1].length; k++) {
                            if (A[i - 1][k].FinishTime < A[i][j].StartTime) {
                                var waitTime = A[i][j].StartTime - A[i - 1][k].FinishTime;
                                var tmpValue = dp[i - 1][k] + waitTime;
                                if (tmpValue < dpItem) {
                                    dpItem = tmpValue;
                                    traceItem = k;
                                }
                            }
                        }
                    }
                    dp[i].push(dpItem);
                    trace[i].push(traceItem);
                }
            }
            // calc temp result
            var tmpMinWait = MAX;
            var jMinWait = -1;
            for (var j = 0; j < dp[n - 1].length; j++) {
                if (dp[n - 1][j] < tmpMinWait) {
                    tmpMinWait = dp[n - 1][j];
                    jMinWait = j;
                }
            }
            // update result
            if (tmpMinWait < minWait) {
                var tmpResult = [];
                var doTrace = function (i, j) {
                    if (i < 0) {
                        return;
                    }
                    doTrace(i - 1, trace[i][j]);
                    var item = {
                        SampleId: sampleIdOrder[i],
                        SlotId: sampleOrderWithSlots[i][j].SlotId,
                    }
                    tmpResult.push(item);
                }
                doTrace(n - 1, jMinWait);
                result = tmpResult;
            }
        }

        var solve = function () {
            prepareDataStructure();
            permute(0);
        }

        solve();

        console.log(minWait/60/60, ' mins');
        return result;

    }// end define CalcTheBestTour
}