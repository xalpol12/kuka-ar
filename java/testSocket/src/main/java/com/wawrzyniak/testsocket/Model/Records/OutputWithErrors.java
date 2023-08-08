package com.wawrzyniak.testsocket.Model.Records;

import java.util.Map;

public record OutputWithErrors(Map<String, Map<String, ValueException>> values, ExceptionMessagePair exception) {
}
