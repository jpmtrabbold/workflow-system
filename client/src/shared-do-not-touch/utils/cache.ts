interface CacheConfig {
    cacheKey: string
}
export const cacheConfig: CacheConfig = {
    cacheKey: "Generic-Cache-Key"
}

export function setCacheKey(cacheKey: string) {
    cacheConfig.cacheKey = cacheKey
}

export interface withCacheParams<T> {
    /**
     * promise or async function that will retrieve the external data silently (with no ui feedback)
     */
    getDataSilently: () => Promise<T>
    /**
     * promise or async function that will retrieve the external data silently (with ui feedback)
     */
    getDataWithLoading: () => Promise<T>
    /**
     * callback for doing stuff with the data (for example, setting state for rendering)
     */
    whatToDoWithData: (data: T) => Promise<any>
    /**
     * whether cache handler has to try to fetch updated data even if there is cache for that. Once fetched, will compare with the cache and, if different, will re-run whatToDoWithData()
     */
    triesToFetchUpdatedData?: boolean
}
/**
 * returns a strongly-typed cache handler for complex objects
 * @param key unique key for this object cache
 * @param transformationToCache callback to transform the data into simple json object that is cacheable
 * @param transformationFromCache callback to transform the json object from cache into the instatiated data
 */
export function getCacheHandler<T>(
    key: string, 
    transformationToCache = (data: T) => data as any, 
    transformationFromCache = (cacheData: any) => cacheData as T) {

    const cacheKey = key + cacheConfig.cacheKey

    /**
     * retrieves external data with cache awareness
     * @param param0 options - see each option for documentation
     */
    async function withCache({ 
        getDataSilently, 
        getDataWithLoading,
        whatToDoWithData, 
        triesToFetchUpdatedData = true }: withCacheParams<T>): Promise<any> {

        let cacheData = get()
        let cache = true
        if (cacheData) {
            await whatToDoWithData(cacheData!)
        } else {
            cache = false
        }

        if (cache && !triesToFetchUpdatedData) {
            return
        }
        
        let getData = getDataSilently
        if (!cache && getDataWithLoading) {
            getData = getDataWithLoading
        }

        let serverData: T | undefined

        try {
            serverData = await getData()
        } catch (error) {
            console.log(error)
            return
        }

        if (serverData !== undefined) {
            const changed = set(serverData!)
            if (changed) {
                whatToDoWithData(serverData)
            }
        }
    }

    const stringifyToCache = (data?: T) => {
        if (!data) {
            return ""
        }
        return JSON.stringify(transformationToCache(data)) 
    }

    const parseFromCache = (cacheData: any) => {
        return transformationFromCache(JSON.parse(cacheData))
    }

    /**
     * set object cache
     * @param data content to be cached
     */
    const set = (data?: T) => {
        const previousContent = localStorage[cacheKey]
        const newContent = stringifyToCache(data)
        if (previousContent === newContent) {
            return false
        }
        localStorage[cacheKey] = newContent
        return true
    }

    /**
     * get object cache
     */
    const get = () => {
        const data = localStorage[cacheKey]
        if (data)
            return parseFromCache(data)
        else
            return null
    }

    return {
        withCache,
        set,
        get
    }
}