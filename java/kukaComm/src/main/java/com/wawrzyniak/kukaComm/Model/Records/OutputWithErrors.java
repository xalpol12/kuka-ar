package com.wawrzyniak.kukaComm.Model.Records;

import java.util.Map;

public record OutputWithErrors(Map<String, Map<String, ValueException>> values, ExceptionMessagePair exception) {
}
