import { useLocation } from "react-router-dom";
import { useMemo } from "react";
import queryString from 'query-string'

export default function useQueryString() {
    const location = useLocation()

    return useMemo(() => {
        return queryString.parse(location.search);
    }, [location])
}