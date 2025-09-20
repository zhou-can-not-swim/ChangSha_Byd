export function convertToKeyValueArray(arr) {
  const map = new Map();
  
  arr.forEach(item => {
    let key, display_name;
    
    if (typeof item === 'object' && item !== null) {
      key = item.id || '';
      display_name = item.name || '';
    } else if (Array.isArray(item) && item.length >= 2) {
      key = item.id || '';
      display_name = item.name || '';
    } else {
      key = String(item);
      display_name = String(item);
    }
    
    const identifier = `${display_name}`;  
    
    // Map 会保持插入顺序，且key唯一
    if (!map.has(identifier)) {
      map.set(identifier, { key, display_name });
    }
  });
  
  // 将Map的值转换为数组
  return Array.from(map.values());
  
}